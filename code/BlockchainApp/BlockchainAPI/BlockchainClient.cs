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
        List<Property> properties;
        public bool userExist;
        private List<Transaction> transactions = new List<Transaction>();
        IBlockchainService blockchainService;
        public string username;

        public BlockchainClient(IBlockchainService blockChain)
        {
            blockchainService = blockChain;
            userExist = false;
            client = new HttpClient();
        }

        public BlockchainClient(string username, IBlockchainService blockChain)
        {
            blockchainService = blockChain;
            userExist = false;
            client = new HttpClient();
            this.username = username;
            
            CheckUserExisting(this.username);

            if (userExist)
            {
                parseTrader();
                updatePropertyList();
                LoadUserTransactions();
            }
        }

        private async void LoadUserTransactions()
        {
            var resultsString = await blockchainService.InvokeGet(HyperledgerConsts.TransactionUrl);

            transactions = JsonConvert.DeserializeObject<List<Transaction>>(resultsString);

            for (int i = transactions.Count - 1; i >= 0; i--)
            {
                if(transactions[i].newOwner.Substring(32) == username)
                {
                    transactions[i].property = transactions[i].property.Substring(34);
                    transactions[i].property = transactions[i].property.Replace("%20", " ");
                }
                else
                {
                    transactions.Remove(transactions[i]);
                }
            }
   
        }

        private void parseTrader()
        {
            var requestURL = blockchainService.GetTraderURL(username);
            var resultsString = GetJsonString(requestURL);

            thisTrader =  JsonConvert.DeserializeObject<Trader>(resultsString);
        }

        public async Task<bool> login(string text)
        {
            try
            {
                var results = await blockchainService.InvokeGet(HyperledgerConsts.TraderQueryURL(text));
                thisTrader = JsonConvert.DeserializeObject<Trader>(results);
                return true;
            }
            catch (System.Net.Http.HttpRequestException e)
            {
                return false;
            }
        }

        private void updatePropertyList()
        {
            string requestURL = blockchainService.GetPropertiesByUserURL(username);
            var stuff = GetJsonString(requestURL);

            properties = JsonConvert.DeserializeObject<List<Property>>(stuff);
        }

        public void CheckUserExisting(string traderID)
        {
            string requestURL = blockchainService.GetTradersURL();
            var resultsString = GetJsonString(requestURL);

            List<Trader> traders = new List<Trader>();       
            traders = JsonConvert.DeserializeObject<List<Trader>>(resultsString);

            foreach (Trader trader in traders)
            {
                if (trader.traderId == traderID)
                    userExist = true;
            }

        }

        public bool sendProperty(String propertyID, String recipientID, String latitude, String longitude)
        {
            Dictionary<String, String> parameters = new Dictionary<string, string>
            {
                { "property",propertyID },
                { "newOwner",recipientID },
                //{ "latitude", latitude},
                //{ "longitude", longitude}
            };

            var results = Task.Run(() => client.PostAsync(blockchainService.GetTransactionsURL(),
                new FormUrlEncodedContent(parameters))).Result;
            //var stringResults = Task.Run(() => results.Content.ReadAsStringAsync());

            return true;
        }

        public void RegisterNewTrader(string UserId, string firstName, string lastName)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                {"traderId", UserId },
                {"firstName", firstName },
                {"lastName", lastName }
            };

            var results = Task.Run(() => client.PostAsync(blockchainService.GetTradersURL(),
                new FormUrlEncodedContent(parameters))).Result;
        }

        public void RegisterNewAsset(string assetID, string description, string ownerID)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                {"PropertyId", assetID },
                {"description", description },
                {"owner", ownerID }
            };

            var results = Task.Run(() => client.PostAsync(blockchainService.GetPropertyURL(),
                new FormUrlEncodedContent(parameters))).Result;
        }

        public string getName()
        {
            return String.Format("{0} {1}", thisTrader.firstName, thisTrader.lastName);
        }

        public string getUserID()
        {
            return thisTrader.traderId;
        }

        public List<Property> getMyAssets()
        {
            return properties;
        }

        public List<Transaction> GetUserTransactions()
        {
            return transactions;
        }

        public String GetJsonString(string Url)
        {
            var results = Task.Run(() => client.GetAsync(Url)).Result;
            return Task.Run(() => results.Content.ReadAsStringAsync()).Result;
        }
    }
}
