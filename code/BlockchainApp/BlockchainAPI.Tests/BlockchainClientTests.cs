using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlockchainAPI.Models;
using Moq;
using System.Threading.Tasks;

namespace BlockchainAPI.Tests
{
    [TestClass]
    public class BlockchainClientTests
    {
        private HyperLedgerComposerBlockChain blockChainService;
        private BlockchainClient client;
        private List<String> propertiesID;

        [TestInitialize]
        public void beforeEach()
        {
            blockChainService = new HyperLedgerComposerBlockChain();
            propertiesID = new List<string>();
        }

        [TestMethod]
        public void CanaryTest()
        {
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void ConstructorOfBlockchainMethodShouldSetUsername()
        {
           client = new BlockchainClient("TRADER1", blockChainService);

            Assert.IsTrue(client.username == "TRADER1");
        }

        [TestMethod]
        public void TestIfRealUserExist()
        {
            client = new BlockchainClient("TRADER1", blockChainService);
            
            client.CheckUserExisting("TRADER1");

            Assert.IsTrue(client.userExist);
        }

        [TestMethod]
        public void TestIfNonUserExist()
        {
            client = new BlockchainClient("unknown", blockChainService);

            client.CheckUserExisting("unknown");

            Assert.IsFalse(client.userExist);
        }

        [TestMethod]
        public void RegisterUserTest()
        {
            client = new BlockchainClient(blockChainService);
            client.RegisterNewTrader("1", "John", "Smith");

            client.CheckUserExisting("1");

            Assert.IsTrue(client.userExist);
        }

        public void GetPropertiesId(BlockchainClient theClient)
        {
            foreach (var property in theClient.getMyAssets())
            {
               propertiesID.Add(property.PropertyId); 
            }
        }

        [TestMethod]
        public void CreateAssetTest()
        {
            bool isContain = false;
            client = new BlockchainClient("TRADER1", blockChainService);

            client.RegisterNewAsset("1234", "test", "TRADER1");
            GetPropertiesId(client);
            foreach (var i in propertiesID)
            {
                if (i == "1234")
                    isContain = true;
            }

            Assert.IsTrue(isContain);
        }

        [TestMethod]
        public void BlockchainServiceShouldDetermineWhatComesBackFromObject()
        {
            var mock = new Mock<IBlockChain>();
            mock.Setup(foo => foo.InvokeGet(It.IsAny<String>()))
                .Returns(Task.FromResult("[]"));

            
            Assert.IsTrue(true);
        }
    }
}
