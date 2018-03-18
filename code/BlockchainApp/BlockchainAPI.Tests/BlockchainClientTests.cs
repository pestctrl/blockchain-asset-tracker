using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlockchainAPI.Tests
{
    [TestClass]
    public class BlockchainClientTests
    {
        [TestMethod]
        public void CanaryTest()
        {
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void ConstructorOfBlockchainMethodShouldSetUsername()
        {
            BlockchainClient client = new BlockchainClient("TRADER1");

            Assert.IsTrue(client.username == "TRADER1");
        }
    }
}
