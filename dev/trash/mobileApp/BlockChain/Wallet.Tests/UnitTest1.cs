using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlockChain;

namespace Wallet.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Canary()
        {
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void SendOneAsset()
        {

            UserWallet wallet = new UserWallet();
            String receiverAddress = wallet.GetReceiverAddress("John address");
            String asset = wallet.SelectedAsset("Asset B");
            String sendMessage = wallet.Send(receiverAddress, asset);
            Assert.AreEqual("Send Asset B to John address", sendMessage);
        }

        [TestMethod]
        public void ReceiveOneAsset()
        {
            UserWallet wallet = new UserWallet();
            wallet.ReceivedAsset("Asset D");
            Assert.AreEqual("Asset D", wallet.SelectedAsset("Asset D"));
        }
    }
}
