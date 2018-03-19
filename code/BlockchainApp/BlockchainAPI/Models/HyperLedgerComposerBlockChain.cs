using System;
using System.Collections.Generic;
using System.Text;

namespace BlockchainAPI.Models
{
    public class HyperLedgerComposerBlockChain : IBlockChain
    {
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
    }
}
