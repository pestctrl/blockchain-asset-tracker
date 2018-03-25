using BlockchainAPI.Models;
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
        HttpClient client;
        public Trader thisTrader;
        IBlockchainService blockchainService;
        public string fullName { get { return String.Format("{0} {1}", thisTrader.firstName, thisTrader.lastName); } }

        public BlockchainClient(IBlockchainService blockChain)
        {
            blockchainService = blockChain;
            client = new HttpClient();
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

        public async Task<bool> login(string text)
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

        public async Task<bool> sendProperty(String propertyID, String recipientID, String latitude, String longitude)
        {
            Dictionary<String, String> parameters = new Dictionary<string, string>
            {
                { "property",propertyID },
                { "newOwner",recipientID },
                //{ "latitude", latitude},
                //{ "longitude", longitude}
            };

            try
            {
                await blockchainService.InvokePost(HyperledgerConsts.TransactionUrl,parameters);
                return true;
            }
            catch(HttpRequestException e)
            {
                return true;
            }
        }

        public async Task<bool> RegisterNewTrader(string UserId, string firstName, string lastName)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                {"traderId", UserId },
                {"firstName", firstName },
                {"lastName", lastName }
            };
            
            try
            {
                await blockchainService.InvokePost(HyperledgerConsts.TraderUrl,parameters);
                return true;
            }
            catch(HttpRequestException e)
            {
                return false;
            }
        }

        public async Task<bool> RegisterNewProperty(string assetID, string description, string ownerID)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                {"PropertyId", assetID },
                {"description", description },
                {"owner", ownerID }
            };
            
            try
            {
                await blockchainService.InvokePost(HyperledgerConsts.PropertyUrl, parameters);
                return true;
            }
            catch(HttpRequestException e)
            {
                return false;
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
            var resultsString = await blockchainService.InvokeGet(HyperledgerConsts.TransactionUrl);

            var transactions = JsonConvert.DeserializeObject<List<Transaction>>(resultsString);

            for (int i = transactions.Count - 1; i >= 0; i--)
            {
                if (transactions[i].newOwner.Substring(32) == thisTrader.traderId)
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

        public String GetJsonString(string Url)
        {
            var results = Task.Run(() => client.GetAsync(Url)).Result;
            return Task.Run(() => results.Content.ReadAsStringAsync()).Result;
        }
    }
}
