using BlockchainAPI.Models;
using BlockchainAPI.Transactions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.IO;
using QRCoder;
using System.Net.Mime;
using System.Diagnostics;

namespace BlockchainAPI
{
    public class SelectedData<T>
    {
        public T data { get; set; }
        public bool selected { get; set; }
    }

    public class BlockchainClient
    {
        public Trader thisTrader;
        public IBlockchainService blockchainService;
        public enum Result {SUCCESS, FAILED, NETWORK, EXISTS}
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

        public async Task<List<Transfer>> GetAllTransfers()
        {
            var results = await blockchainService.InvokeGet(HyperledgerConsts.TransferUrl);
            return JsonConvert.DeserializeObject<List<Transfer>>(results);
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

        public async Task<bool> login(User user)
        {
            try
            {
                var results = await blockchainService.InvokePostAuthentication(FlaskConsts.LoginUrl, JsonConvert.SerializeObject(user));
                var checkUser = JsonConvert.DeserializeObject<messageCredential>(results);

                if (!string.IsNullOrEmpty(checkUser.access_token))
                {
                    var request = HyperledgerConsts.TraderQueryURL(user.username);
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


        public async Task<Result> sendProperty(Transaction transaction)
        {
            try
            {
                bool flag = await UserExists(transaction.newOwner);
                if (flag)
                {
                    await blockchainService.InvokePost(HyperledgerConsts.TransactionUrl, JsonConvert.SerializeObject(transaction));
                    return Result.SUCCESS;
                }
                else { return Result.EXISTS; }
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
                bool flag = await PropertyExists(property.PropertyId);
                if (!flag)
                {
                    await blockchainService.InvokePost(HyperledgerConsts.PropertyUrl, JsonConvert.SerializeObject(property));
                     return Result.SUCCESS;
                }
                else { return Result.EXISTS; }
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
                var results = await blockchainService.InvokePost(HyperledgerConsts.NewTransferUrl, JsonConvert.SerializeObject(t));
                return Result.SUCCESS;
            }
            catch (HttpRequestException)
            {
                return Result.NETWORK;
            }
        }

        public async Task<List<Property>> getMyProperties()
        {
            try
            {
                var results = await blockchainService.InvokeGet(HyperledgerConsts.MyAssetsUrl(thisTrader.traderId));
                return JsonConvert.DeserializeObject<List<Property>>(results);
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public async Task<List<Package>> GetPackage()
        {
            try
            {
                var results = await blockchainService.InvokeGet(HyperledgerConsts.MyPackagesUrl(thisTrader.traderId));
                return JsonConvert.DeserializeObject<List<Package>>(results);
            }
            catch (HttpRequestException e)
            {
                return null;
            }
        }

        public async Task<List<CreatePackage>> GetUserTransactions()
        {
            var resultsString = await blockchainService.InvokeGet(HyperledgerConsts.CreatePackageUrl);

            var transactions = JsonConvert.DeserializeObject<List<CreatePackage>>(resultsString);

            for (int i = transactions.Count - 1; i >= 0; i--)
            {
                
                if (transactions[i].recipient.Substring(35) == thisTrader.traderId || transactions[i].sender.Substring(35) == thisTrader.traderId)
                {
                    for(int j = 0; j < transactions[i].contents.Count; j++)
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

        public async Task<List<Transfer>> GetPropertyHistory(string property)
        {
            List<Transfer> finalList = new List<Transfer>();
            var results = await blockchainService.InvokeGet(HyperledgerConsts.PropertyPackageUrl(Uri.EscapeDataString(property)));
            var list = JsonConvert.DeserializeObject<List<Package>>(results);

            foreach(Package package in list)
            {
                var res2 = await blockchainService.InvokeGet(HyperledgerConsts.PackageHistoryUrl(Uri.EscapeDataString(package.PackageId)));
                var packageHistory = JsonConvert.DeserializeObject<List<Transfer>>(res2);
                finalList.AddRange(packageHistory);
            }

            return finalList.OrderBy(x => x.timestamp).Reverse().ToList();
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactions()
        {
            var results = await blockchainService.InvokeGet(HyperledgerConsts.OrderedTransactionUrl);
            return JsonConvert.DeserializeObject<List<Transaction>>(results);
        }

        public async Task CreatePackage(CreatePackage package)
        {
            //mailtQrCodeToSender(propertyID);
            await blockchainService.InvokePost(HyperledgerConsts.CreatePackageUrl, JsonConvert.SerializeObject(package));
        }

        private void mailtQrCodeToSender(string propertyID)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("BlockChainMessenger@gmail.com");
            mail.To.Add("BlockChainMessenger@gmail.com");
            mail.Subject = "Test Mail - 1";
            mail.Body = "mail with attachment";

            AlternateView av = AlternateView.CreateAlternateViewFromString(mail.Body, null, MediaTypeNames.Text.Html);
            LinkedResource headerImage = new LinkedResource(GenerateQRCode(propertyID), MediaTypeNames.Image.Jpeg);
            headerImage.ContentId = "QRCode";
            headerImage.ContentType = new ContentType("image/jpg");
            av.LinkedResources.Add(headerImage);
            mail.AlternateViews.Add(av);

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("BlockChainMessenger@gmail.com", "riceforlife1");
            SmtpServer.EnableSsl = true;
            SmtpServer.Send(mail);
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
