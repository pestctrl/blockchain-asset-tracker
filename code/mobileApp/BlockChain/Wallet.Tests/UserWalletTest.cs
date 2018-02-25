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
        public void GetVerifiedTransactionOneAsset()
        {
            Mock<IBlockChainService> mockBlockChainService = new Mock<IBlockChainService>();
            List<String> assets = new List<string>() { "Asset B" };
            mockBlockChainService.Setup(x => x.VerifiedTransaction("John address", assets)).Returns(true);
            UserWallet wallet = new UserWallet(mockBlockChainService.Object);
            Tuple<String, List<String>, bool> sendMessage = wallet.GetTransactionVerify("John address", assets);
            Tuple<String, List<String>, bool> expectedResult = new Tuple<string, List<string>, bool>("John address", assets, true);
            Assert.AreEqual(expectedResult, sendMessage);
            
        }

        [TestMethod]
        public void UnvalidatedTransactionOneAsset()
        {
            Mock<IBlockChainService> mockBlockChainService = new Mock<IBlockChainService>();
            List<String> assets = new List<string>() { "Asset B" };
            mockBlockChainService.Setup(x => x.VerifiedTransaction("John address", assets)).Returns(false);
            UserWallet wallet = new UserWallet(mockBlockChainService.Object);
            Tuple<String, List<String>, bool> sendMessage = wallet.GetTransactionVerify("John address", assets);
            Tuple<String, List<String>, bool> expectedResult = new Tuple<string, List<string>, bool>("John address", assets, false);
            Assert.AreEqual(expectedResult, sendMessage);
        }

        [TestMethod]
        public void FailToGetTransactionVerify()
        {
            Mock<IBlockChainService> mockBlockChainService = new Mock<IBlockChainService>();
            List<String> assets = new List<string>() { "Asset B" };
            mockBlockChainService.Setup(x => x.VerifiedTransaction("John address", assets)).Throws(new ApplicationException("network error"));
            UserWallet wallet = new UserWallet(mockBlockChainService.Object);
            Tuple<String, List<String>, bool> verifyTransaction = wallet.GetTransactionVerify("John address", assets);
            Tuple<String, List<String>, bool> expectedResult = new Tuple<string, List<string>, bool>("John address", assets, false);
            Assert.AreEqual(expectedResult, verifyTransaction);
        }

        [TestMethod]
        public void SendSuccessfulOneAccessFromVerifiedTransaction()
        {
            Mock<IBlockChainService> mockBlockChainService = new Mock<IBlockChainService>();
            List<String> assets = new List<string>() { "Asset B" };
            mockBlockChainService.Setup(x => x.VerifiedTransaction("John address", assets)).Returns(true);
            UserWallet wallet = new UserWallet(mockBlockChainService.Object);
            Tuple<String, List<String>, bool> verifiedTransaction = wallet.GetTransactionVerify("John address", assets);
            wallet.SendVerifiedTransaction(verifiedTransaction);
            List<String> expectedResult = new List<string>()
            {
                "Asset A",
                "Asset C"
            };
            Assert.IsTrue(expectedResult.SequenceEqual(wallet.assets));
        }

        [TestMethod]
        public void SendSuccessfulTwoAccessFromVerifiedTransaction()
        {
            Mock<IBlockChainService> mockBlockChainService = new Mock<IBlockChainService>();
            List<String> assets = new List<string>() { "Asset B", "Asset A" };
            mockBlockChainService.Setup(x => x.VerifiedTransaction("John address", assets)).Returns(true);
            UserWallet wallet = new UserWallet(mockBlockChainService.Object);
            Tuple<String, List<String>, bool> verifiedTransaction = wallet.GetTransactionVerify("John address", assets);
            wallet.SendVerifiedTransaction(verifiedTransaction);
            List<String> expectedResult = new List<string>()
            {
                "Asset C"
            };
            Assert.IsTrue(expectedResult.SequenceEqual(wallet.assets));
        }

        [TestMethod]
        public void SendSuccessfulAllAccessFromVerifiedTransaction()
        {
            Mock<IBlockChainService> mockBlockChainService = new Mock<IBlockChainService>();
            List<String> assets = new List<string>() { "Asset B", "Asset A", "Asset C" };
            mockBlockChainService.Setup(x => x.VerifiedTransaction("John address", assets)).Returns(true);
            UserWallet wallet = new UserWallet(mockBlockChainService.Object);
            Tuple<String, List<String>, bool> verifiedTransaction = wallet.GetTransactionVerify("John address", assets);
            wallet.SendVerifiedTransaction(verifiedTransaction);
            List<String> expectedResult = new List<string>();
            Assert.IsTrue(expectedResult.SequenceEqual(wallet.assets));
        }


        [TestMethod]
        public void ReceiveOneAsset()
        {
            UserWallet wallet = new UserWallet();
            wallet.ReceivedAsset(new List<string>() { "Asset D" });
            List<String> ExpectedResult = new List<string>() { "Asset A", "Asset B", "Asset C", "Asset D" };
            Assert.IsTrue(ExpectedResult.SequenceEqual(wallet.assets));
        }

        [TestMethod]
        public void ReceiveTwoAsset()
        {
            UserWallet wallet = new UserWallet();
            wallet.ReceivedAsset(new List<string>() { "Asset D", "Asset E" });
            List<String> ExpectedResult = new List<string>() { "Asset A", "Asset B", "Asset C", "Asset D", "Asset E" };
            Assert.IsTrue(ExpectedResult.SequenceEqual(wallet.assets));
        }
    }
}
