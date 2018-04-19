using BlockchainAPI.Models;
using BlockchainAPI.Transactions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlockchainAPI
{
    public class BlockchainClient
    {
        public Trader thisTrader;
        public IBlockchainService blockchainService;
        public enum Error {SUCCESS, NETWORK, EXISTS}
        public BlockchainClient(IBlockchainService blockChain)
        {
            blockchainService = blockChain;
        }

        public IBlockchainService GetBlockChainService()
        {
            return blockchainService;
        }

        public async Task<bool> userExists(string text)
        {
            try
            {
                var request = HyperledgerConsts.TraderQueryURL(text);
                var results = await blockchainService.InvokeGet(request);
                return true;
            }
            catch (System.Net.Http.HttpRequestException e)
            {
                return false;
            }
        }

        public async Task<bool> PropertyExists(string text)
        {
            try
            {
                var request = HyperledgerConsts.PropertyPackageUrl(text);
                var results = await blockchainService.InvokeGet(request);
                return true;
            }
            catch (System.Net.Http.HttpRequestException e)
            {
                return false;
            }
        }

        public async Task<bool> login(string text, string password)
        {
            try
            {
                var request = HyperledgerConsts.TraderQueryURL(text);
                var results = await blockchainService.InvokeGet(request);
                thisTrader = JsonConvert.DeserializeObject<Trader>(results);
                return true;
            }
            catch (System.Net.Http.HttpRequestException e)
            {
                return false;
            }
        }

        public async Task<Error> RegisterNewTrader(Trader t)
        {
            try
            {
                bool flag = await userExists(t.traderId);
                if (flag)
                {
                    await blockchainService.InvokePost(HyperledgerConsts.TraderUrl, JsonConvert.SerializeObject(t));
                    return Error.SUCCESS;
                }
                else { return Error.EXISTS; }
            }
            catch (HttpRequestException e)
            {
                return Error.NETWORK;
            }
        }

        public async Task<Error> sendProperty(Transaction t)
        {
            try
            {
                bool flag = await userExists(t.newOwner);
                if (flag)
                {
                    await blockchainService.InvokePost(HyperledgerConsts.TransactionUrl, JsonConvert.SerializeObject(t));
                    return Error.SUCCESS;
                }
                else { return Error.EXISTS; }
            }
            catch (HttpRequestException e)
            {
                return Error.NETWORK;
            }
        }

        public async Task<Error> RegisterNewProperty(Property p)
        {
            try
            {
                bool flag = await PropertyExists(p.PropertyId);
                if (flag)
                {
                    await blockchainService.InvokePost(HyperledgerConsts.PropertyUrl, JsonConvert.SerializeObject(p));
                     return Error.SUCCESS;
                }
                else { return Error.EXISTS; }
            }
            catch (HttpRequestException e)
            {
                return Error.NETWORK;
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

        public async Task<List<Transaction>> GetPropertyHistory(string property)
        {
            var results = await blockchainService.InvokeGet(HyperledgerConsts.PropertyPackageUrl(Uri.EscapeDataString(property)));
            var list = JsonConvert.DeserializeObject<List<Package>>(results);
            foreach(Package p in list)
            {
                var packageHistory = await blockchainService.InvokeGet(HyperledgerConsts.PackageHistoryUrl(p.PackageId));
            }
            return JsonConvert.DeserializeObject<List<Transaction>>(results);
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactions()
        {
            var results = await blockchainService.InvokeGet(HyperledgerConsts.OrderedTransactionUrl);
            return JsonConvert.DeserializeObject<List<Transaction>>(results);
        }

        public async Task CreatePackage(CreatePackage p)
        {
            await blockchainService.InvokePost(HyperledgerConsts.CreatePackageUrl, JsonConvert.SerializeObject(p));
        }

        public async Task UnboxPackage(UnboxPackage p)
        {
            await blockchainService.InvokePost(HyperledgerConsts.UnboxPackageUrl, JsonConvert.SerializeObject(p));
        }
    }

}
