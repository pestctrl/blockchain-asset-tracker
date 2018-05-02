using BlockchainAPI.Models;
using BlockchainAPI.Transactions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Mail;
using System.IO;
using QRCoder;
using System.Net.Mime;

namespace BlockchainAPI
{
    public class BlockchainClient
    {
        public Trader thisTrader;
        public IBlockchainService blockchainService;
        public enum Result {SUCCESS, NETWORK, EXISTERROR, EMPTY}

        public BlockchainClient(IBlockchainService blockChain)
        {
            blockchainService = blockChain;
        }

        public IBlockchainService GetBlockChainService()
        {
            return blockchainService;
        }

        public async Task<bool> UserExists(string username)
        {
            try
            {
                return await blockchainService.InvokeHead(HyperledgerConsts.TraderUrl, username);
            }
            catch (System.Net.Http.HttpRequestException)
            {
                return false;
            }
        }

        public async Task<List<NewTransfer>> GetAllTransfers()
        {
            string results = await blockchainService.InvokeGet(HyperledgerConsts.NewTransferUrl);
            return JsonConvert.DeserializeObject<List<NewTransfer>>(results);
        }

        public async Task<bool> PropertyExists(string propertyId)
        {
            try
            {
                return await blockchainService.InvokeHead(HyperledgerConsts.PropertyUrl, propertyId);
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }

        public async Task<bool> Login(User user)
        {
            try
            {
                string results = await blockchainService.InvokePostAuthentication(FlaskConsts.LoginUrl, JsonConvert.SerializeObject(user));
                messageCredential checkUser = JsonConvert.DeserializeObject<messageCredential>(results);

                if (!string.IsNullOrEmpty(checkUser.access_token))
                {
                    string request = HyperledgerConsts.TraderQueryURL(user.username);
                    results = await blockchainService.InvokeGet(request);
                    thisTrader = JsonConvert.DeserializeObject<Trader>(results);
                    return true;
                }
                else
                    return false;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }
        public async Task<Result> RegisterNewTrader(User user)
        {
            try
            {
                await blockchainService.InvokePostAuthentication(FlaskConsts.RegistrationUrl, JsonConvert.SerializeObject(user));
                return Result.SUCCESS;
            }
            catch (HttpRequestException)
            {
                return Result.NETWORK;
            }
        }


        public async Task<Result> SendProperty(Transaction transaction)
        {
            try
            {
                bool flag = await UserExists(transaction.newOwner);
                if (flag)
                {
                    await blockchainService.InvokePost(HyperledgerConsts.TransactionUrl, JsonConvert.SerializeObject(transaction));
                    return Result.SUCCESS;
                }
                else { return Result.EXISTERROR; }
            }
            catch (HttpRequestException)
            {
                return Result.NETWORK;
            }
        }

        public async Task<Result> RegisterNewProperty(Property property)
        {
            try
            {
                if (String.IsNullOrEmpty(property.PropertyId) || String.IsNullOrEmpty(property.description))
                    return Result.EMPTY;

                bool flag = await PropertyExists(property.PropertyId);
                if (!flag)
                {
                    await blockchainService.InvokePost(HyperledgerConsts.PropertyUrl, JsonConvert.SerializeObject(property));
                    return Result.SUCCESS;
                }
                else { return Result.EXISTERROR; }
            }
            catch (HttpRequestException)
            {
                
                return Result.NETWORK;
            }
        }

        public async Task<Result> AddNewTransfer(NewTransfer t)
        {
            try
            {
                string results = await blockchainService.InvokePost(HyperledgerConsts.NewTransferUrl, JsonConvert.SerializeObject(t));
                return Result.SUCCESS;
            }
            catch (HttpRequestException)
            {
                return Result.NETWORK;
            }
        }

        public async Task<List<Property>> GetMyProperties()
        {
            try
            {
                string results = await blockchainService.InvokeGet(HyperledgerConsts.MyAssetsUrl(thisTrader.traderId));
                return JsonConvert.DeserializeObject<List<Property>>(results);
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public async Task<List<Package>> GetMyPackages()
        {
            try
            {
                string results = await blockchainService.InvokeGet(HyperledgerConsts.MyPackagesUrl(thisTrader.traderId));
                return JsonConvert.DeserializeObject<List<Package>>(results);
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public async Task<Package> GetPackageInformation(string packageId)
        {
            try
            {
                string results = await blockchainService.InvokeGet(Flurl.Url.Combine(HyperledgerConsts.PackageUrl,packageId));
                return JsonConvert.DeserializeObject<Package>(results);
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public async Task<List<CreatePackage>> GetUserTransactions()
        {
            string resultsString = await blockchainService.InvokeGet(HyperledgerConsts.CreatePackageUrl);
            List<CreatePackage> transactions = JsonConvert.DeserializeObject<List<CreatePackage>>(resultsString);

            for (int i = transactions.Count - 1; i >= 0; i--)
            {
                if (transactions[i].recipient.Substring(35) == thisTrader.traderId || transactions[i].sender.Substring(35) == thisTrader.traderId)
                {
                    for (int j = 0; j < transactions[i].contents.Count; j++)
                    {
                        transactions[i].contents[j] = transactions[i].contents[j].Substring(37);
                        transactions[i].contents[j] = transactions[i].contents[j].Replace("%20", " ");
                    }
                }
                else
                {
                    transactions.Remove(transactions[i]);
                }
            }

            return transactions;
        }

        public async Task<List<NewTransfer>> GetPropertyHistory(string property)
        {
            List<NewTransfer> finalList = new List<NewTransfer>();
            string results = await blockchainService.InvokeGet(HyperledgerConsts.PropertyPackageUrl(Uri.EscapeDataString(property)));
            List<Package> list = JsonConvert.DeserializeObject<List<Package>>(results);

            foreach(Package package in list)
            {
                string res2 = await blockchainService.InvokeGet(HyperledgerConsts.PackageHistoryUrl(Uri.EscapeDataString(package.PackageId)));
                List<NewTransfer> packageHistory = JsonConvert.DeserializeObject<List<NewTransfer>>(res2);
                finalList.AddRange(packageHistory);
            }

            return finalList.OrderBy(x => x.timestamp).Reverse().ToList();
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactions()
        {
            string results = await blockchainService.InvokeGet(HyperledgerConsts.OrderedTransactionUrl);
            return JsonConvert.DeserializeObject<List<Transaction>>(results);
        }

        public async Task<Result> CreatePackage(CreatePackage package)
        {
            try
            {
                bool flag = await UserExists(package.recipient);
                if (!flag)
                {
                    string results = await blockchainService.InvokePost(HyperledgerConsts.CreatePackageUrl, JsonConvert.SerializeObject(package));
                    return Result.SUCCESS;
                }
                else
                {
                    return Result.EXISTERROR;
                }
            }
            catch (HttpRequestException)
            {
                return Result.NETWORK;
            }
        }

        public async Task SendQRCode(string emailAddress, string propertyID)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("BlockChainMessenger@gmail.com");
            mail.To.Add(emailAddress);
            mail.Subject = "Test Mail - 1";
            mail.Body = "mail with attachment";

            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(mail.Body, null, MediaTypeNames.Text.Html);
            LinkedResource headerImage = new LinkedResource(GenerateQRCode(propertyID), MediaTypeNames.Image.Jpeg);
            headerImage.ContentId = "QRCode";
            headerImage.ContentType = new ContentType("image/jpg");
            alternateView.LinkedResources.Add(headerImage);
            mail.AlternateViews.Add(alternateView);

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("BlockChainMessenger@gmail.com", "riceforlife1");
            SmtpServer.EnableSsl = true;
            await SmtpServer.SendMailAsync(mail);
        }

        public async Task UnboxPackage(UnboxPackage package)
        {
            await blockchainService.InvokePost(HyperledgerConsts.UnboxPackageUrl, JsonConvert.SerializeObject(package));
        }

        public MemoryStream GenerateQRCode(string code)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
            
            BitmapByteQRCode bitmapByte = new BitmapByteQRCode(qrCodeData);
            
            return new MemoryStream(bitmapByte.GetGraphic(20));
        }
    }

}
