using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlockchainAPI.Models;
using Moq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace BlockchainAPI.Tests
{
    [TestClass]
    public class BlockchainClientTests
    {
        private BlockchainClient clientWithMock;
        Mock<IBlockchainService> mockBlockService;

        [TestInitialize()]
        public void MockSetup()
        {
            mockBlockService = new Mock<IBlockchainService>();
            clientWithMock = new BlockchainClient(mockBlockService.Object);
        }

        [TestMethod]
        public void CanaryTest()
        {
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task Login_Method_for_client_should_set_username()
        {
            mockBlockService.Setup(m => m.InvokeGet(It.IsAny<String>()))
                            .ReturnsAsync(TestJsonObjectConsts.Trader1);

            await clientWithMock.login(TestJsonObjectConsts.Trader1ID, "");

            Assert.AreEqual(clientWithMock.thisTrader.traderId, TestJsonObjectConsts.Trader1ID);
        }

        [TestMethod]
        public async Task Login_Method_will_call_invokeget_to_check_for_users_existance()
        {
            mockBlockService.Setup(m => m.InvokeGet(It.IsAny<String>()))
                            .ReturnsAsync("{}");

            await clientWithMock.login(TestJsonObjectConsts.Trader1ID, "");

            mockBlockService.Verify(m => m.InvokeGet(HyperledgerConsts.TraderQueryURL(TestJsonObjectConsts.Trader1ID)));
        }

        [TestMethod]
        public async Task If_username_does_not_exist_then_login_return_false()
        {
            mockBlockService.Setup(m => 
                                    m.InvokeGet(HyperledgerConsts.TraderQueryURL(TestJsonObjectConsts.Trader1ID)))
                            .ThrowsAsync(new System.Net.Http.HttpRequestException());

            var loginSuccess = await clientWithMock.login(TestJsonObjectConsts.Trader1ID, "");

            Assert.IsFalse(loginSuccess);
        }

        [TestMethod]
        public async Task Failed_logins_will_also_not_set_trader_object()
        {
            mockBlockService.Setup(m =>
                                    m.InvokeGet(HyperledgerConsts.TraderQueryURL(TestJsonObjectConsts.Trader1ID)))
                            .ThrowsAsync(new HttpRequestException());

            var results = await clientWithMock.login(TestJsonObjectConsts.Trader1ID, "");

            Assert.IsNull(clientWithMock.thisTrader);
        }

        public void AssertTradersEqual(Trader t1, Trader t2)
        {
            Assert.AreEqual(t1.firstName, t2.firstName);
            Assert.AreEqual(t1.lastName, t2.lastName);
            Assert.AreEqual(t1.objectType, t2.objectType);
            Assert.AreEqual(t1.traderId, t2.traderId);
        }
        
        [TestMethod]
        public async Task Successful_login_will_set_trader_object_to_serialized_json()
        {
            var expectedTrader = JsonConvert.DeserializeObject<Trader>(TestJsonObjectConsts.Trader1);
            mockBlockService.Setup(m =>
                                    m.InvokeGet(HyperledgerConsts.TraderQueryURL(TestJsonObjectConsts.Trader1ID)))
                            .ReturnsAsync(TestJsonObjectConsts.Trader1);

            var results = await clientWithMock.login(TestJsonObjectConsts.Trader1ID, "");

            AssertTradersEqual(expectedTrader,clientWithMock.thisTrader);
        }
        
        [TestMethod]
        public async Task Get_My_Assets_Will_InvokeGet_From_BlockchainService()
        {
            mockBlockService.Setup(m =>
                                    m.InvokeGet(HyperledgerConsts.MyAssetsUrl(TestJsonObjectConsts.Trader1ID)))
                            .ReturnsAsync("[]");
            mockBlockService.Setup(m =>
                                    m.InvokeGet(HyperledgerConsts.TraderQueryURL(TestJsonObjectConsts.Trader1ID)))
                            .ReturnsAsync(TestJsonObjectConsts.Trader1);

            await clientWithMock.login(TestJsonObjectConsts.Trader1ID, "");
            var results = await clientWithMock.getMyProperties();

            mockBlockService.Verify(m => m.InvokeGet(HyperledgerConsts.TraderQueryURL(TestJsonObjectConsts.Trader1ID)));
        }
    }
}
