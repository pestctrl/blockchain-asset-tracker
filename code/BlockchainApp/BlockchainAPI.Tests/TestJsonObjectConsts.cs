using System;
using System.Collections.Generic;
using System.Text;

namespace BlockchainAPI.Tests
{
    class TestJsonObjectConsts
    {
        public const String Trader1ID = "TRADER1";
        public const String Trader1 = "{\"$class\": \"org.acme.biznet.Trader\",\"traderId\": \"TRADER1\",\"firstName\": \"Alice\",\"lastName\": \"Margatroid\"}";
        public const String Trader2ID = "TRADER2";
        public const String Trader2 = "{\"$class\": \"org.acme.biznet.Trader\",\"traderId\": \"TRADER2\",\"firstName\": \"Patchouli\",\"lastName\": \"Knowledge\"}";
        public const String listOfProperties = "[{\"$class\": \"org.acme.biznet.Property\",\"PropertyId\": \"123\",\"description\": \"test\",\"owner\": \"resource:org.acme.biznet.Trader#TRADER1\"},{\"$class\": \"org.acme.biznet.Property\",\"PropertyId\": \"1234\",\"description\": \"test\",\"owner\": \"resource:org.acme.biznet.Trader#TRADER1\"}]";
        public const String listOfTransactions = "[{\"$class\": \"org.acme.biznet.Trade\",\"property\": \"resource:org.acme.biznet.Property#Asset%20GB\",\"newOwner\": \"resource:org.acme.biznet.Trader#TRADER2\",\"transactionId\": \"055497bf18712a1ed77b39b66233211e77f1cdb3e15bdc31d3d3edc29de70ec9\",\"timestamp\": \"2018-03-18T10:07:36.029Z\"},{\"$class\": \"org.acme.biznet.Trade\",\"property\": \"resource:org.acme.biznet.Property#Asset%20BB\",\"newOwner\": \"resource:org.acme.biznet.Trader#TRADER1\",\"transactionId\": \"1557853b8c62d5deee43abe5e04c96cc10087bc4b857fac3c105198d43a2d420\",\"timestamp\": \"2018-03-20T00:43:33.213Z\"}]";
        public const String resultTransactionOfTrader2 = "[{\"$class\": \"org.acme.biznet.Trade\",\"property\": \"resource:org.acme.biznet.Property#Asset%20GB\",\"newOwner\": \"resource:org.acme.biznet.Trader#TRADER2\",\"transactionId\": \"055497bf18712a1ed77b39b66233211e77f1cdb3e15bdc31d3d3edc29de70ec9\",\"timestamp\": \"2018-03-18T10:07:36.029Z\"}]";
        public const String Trader2TransactionId = "055497bf18712a1ed77b39b66233211e77f1cdb3e15bdc31d3d3edc29de70ec9";
    }
}
