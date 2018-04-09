using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlockchainAPI
{
    public class HyperledgerConsts
    {
        public const String ipAddress = "http://129.213.108.205:3000";
        public const String TraderUrl = "/api/org.example.biznet.Trader";
        public const String TransactionUrl = "/api/org.example.biznet.Trade";
        public const String PropertyUrl = "/api/org.example.biznet.Property";
        public const String MyAssetsFormatString = "/api/queries/MyProperties?ownerParam=resource%3Aorg.example.biznet.Trader%23{0}";
        public const String OrderedTransactionUrl = "/api/queries/OrderedTransactions";
        public const String PropertyHistoryFormatString = "/api/queries/PropertyHistory?propId=resource%3Aorg.example.biznet.Property%23{0}";

        public static String TraderQueryURL(string username)
        {
            return Flurl.Url.Combine(TraderUrl, username);
        }

        public static String MyAssetsUrl(string username)
        {
            return String.Format(MyAssetsFormatString, username);
        }

        public static string PropertyHistoryUrl(string property)
        {
            return String.Format(PropertyHistoryFormatString, Uri.EscapeDataString(property));
        }
    }
}
