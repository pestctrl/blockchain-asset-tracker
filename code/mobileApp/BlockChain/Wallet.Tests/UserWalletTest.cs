using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlockChain.Models;
using Moq;
using System.Collections.Generic;
using System.Linq;
using BlockChain;

namespace Wallet.Tests
{
    [TestClass]
    public class UserWalletTest
    {
        [TestMethod]
        public void Canary()
        {
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void ReceiveOneAsset()
        {
            UserWallet wallet = new UserWallet();
            wallet.ReceiveAssets(new List<string>() { "Asset D" });

            List<String> expectedResult = new List<string>()
            {
                "Asset D"
            };
            Assert.IsTrue(expectedResult.SequenceEqual(wallet.Assets));
        }

        [TestMethod]
        public void ReceiveTwoAssets()
        {
            UserWallet wallet = new UserWallet();
            wallet.ReceiveAssets(new List<string>() { "Asset D", "Asset E" });

            List<String> expectedResult = new List<string>()
            {
                "Asset D",
                "Asset E"
            };
            Assert.IsTrue(expectedResult.SequenceEqual(wallet.Assets));
        }

        [TestMethod]
        public void GetVerifiedTransactionOneAsset()
        {
            Mock<IBlockChainService> mockBlockChainService = new Mock<IBlockChainService>();
            var assetB = new List<string>() { "Asset B" };
            mockBlockChainService.Setup(x => x.VerifiedTransaction("John address", assetB)).Returns(true);

            UserWallet wallet = new UserWallet(mockBlockChainService.Object);
            var sendMessage = wallet.VerifyTransaction("John address", assetB);

            var expectedResult = new Tuple<string, List<string>, bool>("John address", assetB, true);
            Assert.AreEqual(expectedResult, sendMessage);
        }

        [TestMethod]
        public void UnvalidatedTransactionOneAsset()
        {
            Mock<IBlockChainService> mockBlockChainService = new Mock<IBlockChainService>();
            var assetB = new List<string>() { "Asset B" };
            mockBlockChainService.Setup(x => x.VerifiedTransaction("John address", assetB)).Returns(false);

            UserWallet wallet = new UserWallet(mockBlockChainService.Object);
            var sendMessage = wallet.VerifyTransaction("John address", assetB);

            var expectedResult = new Tuple<string, List<string>, bool>("John address", assetB, false);
            Assert.AreEqual(expectedResult, sendMessage);
        }

        [TestMethod]
        public void SendSuccessfulOneAccessFromVerifiedTransaction()
        {
            UserWallet wallet = new UserWallet();
            var assets = new List<string>()
            {
                "Asset B",
                "Asset A"
            };

            var sendAsset = new List<string>()
            {
                {"Asset A"}
            };

            wallet.ReceiveAssets(assets);
            var verifiedTransaction = new Tuple<String, List<String>, bool> ("John address", sendAsset, true);
            wallet.SendVerifiedTransaction(verifiedTransaction);

            var expectedResult = new List<string>()
            {
                "Asset B",
            };
            Assert.IsTrue(expectedResult.SequenceEqual(wallet.Assets));
        }

        [TestMethod]
        public void SendSuccessfulTwoAccessFromVerifiedTransaction()
        {
            UserWallet wallet = new UserWallet();
            var assets = new List<string>()
            {
                "Asset B",
                "Asset A",
                "Asset C"
            };

            var sendAssets = new List<string>()
            {
                "Asset B",
                "Asset A"
            };

            wallet.ReceiveAssets(assets);
            var verifiedTransaction = new Tuple<String, List<String>, bool>("John address", sendAssets, true);
            wallet.SendVerifiedTransaction(verifiedTransaction);

            var expectedResult = new List<string>()
            {
                "Asset C",
            };
            Assert.IsTrue(expectedResult.SequenceEqual(wallet.Assets));
        }

        [TestMethod]
        public void SendSuccessfulAllAccessFromVerifiedTransaction()
        {
            UserWallet wallet = new UserWallet();
            var assets = new List<string>()
            {
                "Asset B",
                "Asset A",
                "Asset C",
            };

            var sendAssets = new List<string>()
            {
                "Asset B",
                "Asset A",
                "Asset C"
            };

            wallet.ReceiveAssets(assets);
            var verifiedTransaction = new Tuple<String, List<String>, bool>("John address", sendAssets, true);
            wallet.SendVerifiedTransaction(verifiedTransaction);

            var expectedResult = new List<String>();
            Assert.IsTrue(expectedResult.SequenceEqual(wallet.Assets));
        }  
    }
}
