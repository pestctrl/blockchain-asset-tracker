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
        public string username;
        Trader thisTrader;
        List<Property> properties;
        public bool userExist;
        private List<Transaction> transactions = new List<Transaction>();
        IBlockChain blockChainService;

        public BlockchainClient(string username, IBlockChain blockChain)
        {
            blockChainService = blockChain;
            userExist = false;
            client = new HttpClient();
            this.username = username;
            
            CheckUserExisting();

            if (userExist)
            {
                parseTrader();
                updatePropertyList();
                LoadUserTransactions();
            }
        }

        private void LoadUserTransactions()
        {
            var requestURL = blockChainService.GetTransactionsURL();
            var results = Task.Run(() => client.GetAsync(requestURL)).Result;
            var resultsString = Task.Run(() => results.Content.ReadAsStringAsync()).Result;

            transactions = JsonConvert.DeserializeObject<List<Transaction>>(resultsString);

            for (int i = transactions.Count - 1; i >= 0; i--)
            {
                if(transactions[i].newOwner.Substring(32) == username)
                {
                    transactions[i].property = transactions[i].property.Substring(34);
                    transactions[i].property = transactions[i].property.Replace("%20", " ");
                }else
                {
                    transactions.Remove(transactions[i]);
                }
            }
   
        }

        private void parseTrader()
        {
            var requestURL = blockChainService.GetTraderURL(username);
            var results = Task.Run(() => client.GetAsync(requestURL)).Result;

            var resultsString = Task.Run(() => results.Content.ReadAsStringAsync()).Result;
            thisTrader =  JsonConvert.DeserializeObject<Trader>(resultsString);
        }

        public Task login(string text)
        {
            throw new NotImplementedException();
        }

        private void updatePropertyList()
        {
            string requestURL = blockChainService.GetPropertiesByUserURL(username);
            var results = Task.Run(() => client.GetAsync(requestURL)).Result;
            var stuff = Task.Run(() => results.Content.ReadAsStringAsync()).Result;
            properties = JsonConvert.DeserializeObject<List<Property>>(stuff);
        }

        private void CheckUserExisting()
        {
            string requestURL = blockChainService.GetTradersURL();
            var results = Task.Run(() => client.GetAsync(requestURL)).Result;
            var resultsString = Task.Run(() => results.Content.ReadAsStringAsync()).Result;

            List<Trader> traders = new List<Trader>();       
            traders = JsonConvert.DeserializeObject<List<Trader>>(resultsString);

            foreach (Trader trader in traders)
            {
                if (trader.traderId == username)
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

            var results = Task.Run(() => client.PostAsync(blockChainService.GetTransactionsURL(),
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

            var results = Task.Run(() => client.PostAsync(blockChainService.GetTradersURL(),
                new FormUrlEncodedContent(parameters))).Result;
            
        }

        public string getName()
        {
            return String.Format("{0} {1}", thisTrader.firstName, thisTrader.lastName);
        }

        public List<Property> getMyAssets()
        {
            return properties;
        }

        public List<Transaction> GetUserTransactions()
        {
            return transactions;
        }
    }
}
