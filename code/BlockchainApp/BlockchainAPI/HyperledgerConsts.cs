using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlockchainAPI
{
    public class HyperledgerConsts
    {
        public const String ipAddress = "http://129.213.87.202:3000";
        public const String TraderUrl = "/api/org.example.biznet.Trader";
        public const String TransactionUrl = "/api/org.example.biznet.Trade";
        public const String PropertyUrl = "/api/org.example.biznet.Property";
        public const String MyAssetsFormatString = "/api/queries/MyProperties?ownerParam=resource%3Aorg.example.biznet.Trader%23{0}";
        public const String OrderedTransactionUrl = "/api/queries/OrderedTransactions";
        public const String PropertyHistoryFormatString = "/api/queries/PropertyHistory?propId=resource%3Aorg.example.biznet.Property%23{0}";
        public const String PropertyPackageFormatString = "/api/queries/GetPackagesOfProperty?propId=resource%3Aorg.example.biznet.Property%23{0}";
        public const String PackageHistoryFormatString = "/api/queries/GetTransfersOfPackage?packageId=resource%3Aorg.example.biznet.Package%23{0}";
        public const String CreatePackageUrl = "/api/org.example.biznet.CreatePackage";
        public const String UnboxPackageUrl = "/api/org.example.biznet.UnboxPackage";

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

        public static String PropertyPackageUrl(string property)
        {
            return String.Format(PropertyPackageFormatString, property);
        }

        public static String PackageHistoryUrl(string package)
        {
            return String.Format(PackageHistoryFormatString, package);
        }
    }
}
