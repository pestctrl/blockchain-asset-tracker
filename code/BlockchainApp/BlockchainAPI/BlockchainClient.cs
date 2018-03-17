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

        public BlockchainClient(string username)
        {
            client = new HttpClient();
            properties = new List<Property>();
            this.username = username;
            parseTrader();
            updatePropertyList();
        }

        private void parseTrader()
        {
            var requestURL = String.Format("http://129.213.108.205:3000/api/org.acme.biznet.Trader/{0}", username);
            var results = Task.Run(() => client.GetAsync(requestURL)).Result;

            var resultsString = Task.Run(() => results.Content.ReadAsStringAsync()).Result;
            thisTrader =  JsonConvert.DeserializeObject<Trader>(resultsString);
        }

        private void updatePropertyList()
        {
            string requestURL = String.Format("http://129.213.108.205:3000/api/org.acme.biznet.Property?filter=%7B%22where%22%3A%20%7B%22owner%22%3A%20%22resource%3Aorg.acme.biznet.Trader%23{0}%22%7D%7D",
                username);
            var results = Task.Run(() => client.GetAsync(requestURL)).Result;
            var stuff = Task.Run(() => results.Content.ReadAsStringAsync()).Result;

            stuff = stuff.Substring(1, stuff.Length - 2);
            foreach (String s in Regex.Split(stuff, @"(?<=\}),"))
            {
                properties.Add(JsonConvert.DeserializeObject<Property>(s));
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

            var results = Task.Run(() => client.PostAsync("http://129.213.108.205:3000/api/org.acme.biznet.Trade",
                new FormUrlEncodedContent(parameters))).Result;
            var stringResults = Task.Run(() => results.Content.ReadAsStringAsync());

            return true;
        }

        public string getName()
        {
            return String.Format("{0} {1}", thisTrader.firstName, thisTrader.lastName);
        }

        public List<Property> getMyAssets()
        {
            return properties;
        }
    }
}
