using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainAPI.Models
{
    public class HyperLedgerComposerBlockChain : IBlockchainService
    {
        HttpClient client;

        public HyperLedgerComposerBlockChain()
        {
            client = new HttpClient();
        }

        public string GetPropertiesByUserURL(string userName)
        {
            return String.Format("http://129.213.108.205:3000/api/queries/MyAssets" +
                "?ownerParam=resource%3Aorg.acme.biznet.Trader%23{0}",
                userName);
        }

        public string GetTradersURL()
        {
            return "http://129.213.108.205:3000/api/org.acme.biznet.Trader";
        }

        public string GetTraderURL(string userName)
        {
            return String.Format("http://129.213.108.205:3000/api/org.acme.biznet.Trader/{0}", userName);
        }

        public string GetTransactionsURL()
        {
            return "http://129.213.108.205:3000/api/org.acme.biznet.Trade"; 
        }

        public string GetPropertyURL()
        {
            return "http://129.213.108.205:3000/api/org.acme.biznet.Property";
        }

        public async Task<String> InvokeGet(String url)
        {
            var response = await client.GetAsync(Path.Combine(HyperledgerConsts.ipAddress, url));
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> InvokeGet(string url, object parameter)
        {
            var response = await client.GetAsync(Path.Combine(HyperledgerConsts.ipAddress, url, parameter.ToString()));
            return await response.Content.ReadAsStringAsync();
        }
    }
}
