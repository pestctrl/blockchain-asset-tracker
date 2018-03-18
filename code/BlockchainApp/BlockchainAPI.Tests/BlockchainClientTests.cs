using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlockchainAPI.Models;

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
            HyperLedgerComposerBlockChain blockChainService = new HyperLedgerComposerBlockChain();
            BlockchainClient client = new BlockchainClient("TRADER1", blockChainService);

            Assert.IsTrue(client.username == "TRADER1");
        }
    }
}
