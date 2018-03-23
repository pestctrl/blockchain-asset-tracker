using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlockchainAPI.Models;
using Moq;
using System.Threading.Tasks;
using System.Net.Http;

namespace BlockchainAPI.Tests
{
    [TestClass]
    public class BlockchainClientTests
    {
        private IBlockchainService blockchainService;
        private BlockchainClient client;
        Mock<IBlockchainService> mockBlockService;
        string testUsername1 = "Hello";

        [TestInitialize()]
        public void MockSetup()
        {
            mockBlockService = new Mock<IBlockchainService>();
        }

        [TestMethod]
        public void CanaryTest()
        {
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task Login_Method_for_client_should_set_username()
        {
            client = new BlockchainClient(new Mock<IBlockchainService>().Object);

            await client.login(testUsername1);

            Assert.IsTrue(client.username == testUsername1);
        }

        [TestMethod]
        public async Task Login_Method_will_call_invokeget_to_check_for_users_existance()
        {
            mockBlockService.Setup(m => m.InvokeGet(It.IsAny<String>())).Returns(Task.FromResult("{}"));
            client = new BlockchainClient(mockBlockService.Object);

            await client.login(testUsername1);

            mockBlockService.Verify(m => m.InvokeGet(HyperledgerConsts.TraderQueryURL(testUsername1)));
        }

        [TestMethod]
        public async Task If_username_does_not_exist_then_login_return_false()
        {
            var mockBlockService = new Mock<IBlockchainService>();
            mockBlockService.Setup(m => 
                                    m.InvokeGet(HyperledgerConsts.TraderQueryURL(testUsername1)))
                            .ThrowsAsync(new System.Net.Http.HttpRequestException());
            client = new BlockchainClient(mockBlockService.Object);

            var loginSuccess = await client.login(testUsername1);

            Assert.IsFalse(loginSuccess);
        }

        [TestMethod]
        public async Task Failed_logins_will_also_not_set_username()
        {
            var mockBlockService = new Mock<IBlockchainService>();
            mockBlockService.Setup(m =>
                                    m.InvokeGet(HyperledgerConsts.TraderQueryURL(testUsername1)))
                            .ThrowsAsync(new HttpRequestException());
            client = new BlockchainClient(mockBlockService.Object);

            var results = await client.login(testUsername1);

            Assert.IsTrue(client.username != testUsername1);
        }
    }
}
