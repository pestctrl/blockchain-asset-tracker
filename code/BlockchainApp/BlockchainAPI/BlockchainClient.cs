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
            catch (System.Net.Http.HttpRequestException e)
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
            catch (System.Net.Http.HttpRequestException e)
            {
                return false;
            }
        }

        public async Task<bool> login(FlaskUser t)
        {
            try
            {
                var results = await blockchainService.InvokePostFlask(FlaskConsts.LoginUrl, JsonConvert.SerializeObject(t));
                var checkUser = JsonConvert.DeserializeObject<messageCredential>(results);

                if (!string.IsNullOrEmpty(checkUser.access_token))
                {
                    var request = HyperledgerConsts.TraderQueryURL(t.username);
                    results = await blockchainService.InvokeGet(request);
                    thisTrader = JsonConvert.DeserializeObject<Trader>(results);
                    return true;
                }
                else
                    return false;
                   
                
            }
            catch (System.Net.Http.HttpRequestException e)
            {
                return false;
            }
        }


        public async Task<bool> FlaskLogin(string text, string password)
        {
            try
            {
                var request = FlaskConsts.LoginUrl;
                var results = await blockchainService.InvokeGetFlask(request);
                thisTrader = JsonConvert.DeserializeObject<Trader>(results);
                return true;
            }
            catch (System.Net.Http.HttpRequestException e)
            {
                return false;
            }
        }



        public async Task<Result> RegisterNewTrader(Trader t)

        {
            try
            {
                bool flag = await UserExists(t.traderId);
                if (!flag)
                {
                    await blockchainService.InvokePost(HyperledgerConsts.TraderUrl, JsonConvert.SerializeObject(t));
                    return Result.SUCCESS;
                }
                else { return Result.EXISTS; }
            }
            catch (HttpRequestException e)
            {
                return Result.NETWORK;
            }
        }

        public async Task<Result> FlaskRegister(FlaskUser t)
        {
            try
            {
                await blockchainService.InvokePostFlask(FlaskConsts.RegistrationUrl, JsonConvert.SerializeObject(t));
                return Result.SUCCESS;
            }
            catch (HttpRequestException e)
            {
                return Result.NETWORK;
            }
        }


        public async Task<Result> sendProperty(Transaction t)
        {
            try
            {
                bool flag = await UserExists(t.newOwner);
                if (!flag)
                {
                    await blockchainService.InvokePost(HyperledgerConsts.TransactionUrl, JsonConvert.SerializeObject(t));
                    return Result.SUCCESS;
                }
                else { return Result.EXISTS; }
            }
            catch (HttpRequestException e)
            {
                return Result.NETWORK;
            }
        }

        public async Task<Result> RegisterNewProperty(Property p)
        {
            try
            {
                bool flag = await PropertyExists(p.PropertyId);
                if (!flag)
                {
                    await blockchainService.InvokePost(HyperledgerConsts.PropertyUrl, JsonConvert.SerializeObject(p));
                     return Result.SUCCESS;
                }
                else { return Result.EXISTS; }
            }
            catch (HttpRequestException e)
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
            catch (HttpRequestException e)
            {
                return null;
            }
        }

        public async Task<List<Transaction>> GetUserTransactions()
        {
            var resultsString = await blockchainService.InvokeGet(HyperledgerConsts.OrderedTransactionUrl);

            var transactions = JsonConvert.DeserializeObject<List<Transaction>>(resultsString);

            for (int i = transactions.Count - 1; i >= 0; i--)
            {
                if (transactions[i].newOwner.Substring(32) == thisTrader.traderId || transactions[i].origOwner.Substring(32) == thisTrader.traderId)
                {
                    transactions[i].property = transactions[i].property.Substring(34);
                    transactions[i].property = transactions[i].property.Replace("%20", " ");
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

            foreach(Package p in list)
            {
                var res2 = await blockchainService.InvokeGet(HyperledgerConsts.PackageHistoryUrl(Uri.EscapeDataString(p.PackageId)));
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

        public async Task CreatePackage(CreatePackage p, string propertyID)
        {

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("BlockChainMessenger@gmail.com");
            mail.To.Add("bensonchu457@gmail.com");
            mail.Subject = "Test Mail - 1";
            mail.Body = "mail with attachment";


            AlternateView av = AlternateView.CreateAlternateViewFromString(mail.Body, null, MediaTypeNames.Text.Html);
            LinkedResource headerImage = new LinkedResource(GenerateQRCode(propertyID), MediaTypeNames.Image.Jpeg);
            headerImage.ContentId = "QRCode";
            headerImage.ContentType = new ContentType("image/jpg");
            av.LinkedResources.Add(headerImage);
            mail.AlternateViews.Add(av);



            //ContentType mimeType = new System.Net.Mime.ContentType("text/html");
            //AlternateView alternate = AlternateView.CreateAlternateViewFromString(mail.Body, mimeType);
            //mail.AlternateViews.Add(alternate);

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("BlockChainMessenger@gmail.com", "riceforlife1");
            SmtpServer.EnableSsl = true;
            SmtpServer.Send(mail);

            // Send guid to bensonchu457@gmail.com
            await blockchainService.InvokePost(HyperledgerConsts.CreatePackageUrl, JsonConvert.SerializeObject(p));
        }

        public async Task UnboxPackage(UnboxPackage p)
        {
            await blockchainService.InvokePost(HyperledgerConsts.UnboxPackageUrl, JsonConvert.SerializeObject(p));
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
