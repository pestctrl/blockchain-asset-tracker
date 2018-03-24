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
    }
}
