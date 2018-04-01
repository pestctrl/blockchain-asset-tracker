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

            AssertTradersEqual(expectedTrader, clientWithMock.thisTrader);
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

        [TestMethod]
        public async Task Trader_object_has_extra_fullname_field_for_convenience()
        {
            Trader t = new Trader();
            t.firstName = "Hello";
            t.lastName = "World";

            Assert.AreEqual(t.fullName, String.Format("{0} {1}", t.firstName, t.lastName));
        }

        [TestMethod]
        public async Task Existance_of_user_will_InvokeGet_on_TraderQueryURL()
        {
            mockBlockService.Setup(m => m.InvokeGet(It.IsAny<String>()))
                            .ReturnsAsync(JsonConvert.SerializeObject(new Trader()));

            var expectedUrl = HyperledgerConsts.TraderQueryURL(TestJsonObjectConsts.Trader1ID);
            await clientWithMock.userExists(TestJsonObjectConsts.Trader1ID);

            mockBlockService.Verify(m => m.InvokeGet(expectedUrl));
        }

        [TestMethod]
        public async Task If_trader_was_not_found_return_false()
        {
            mockBlockService.Setup(m => m.InvokeGet(It.IsAny<String>()))
                            .ThrowsAsync(new HttpRequestException());

            var results = await clientWithMock.userExists("");

            Assert.IsFalse(results);
        }

        [TestMethod]
        public async Task RegisterNewTrader_will_invoke_post()
        {
            var expectedUrl = HyperledgerConsts.TraderUrl;
            Trader t = new Trader();
            t.traderId = "a";
            t.firstName = "b";
            t.lastName = "c";

            await clientWithMock.RegisterNewTrader(t);

            mockBlockService.Verify(m => m.InvokePost(expectedUrl, JsonConvert.SerializeObject(t)));
        }

        [TestMethod]
        public async Task RegisterNewProperty_will_invoke_post()
        {
            var expectedUrl = HyperledgerConsts.PropertyUrl;

            Property p = new Property();
            p.PropertyId = "a";
            p.description = "b";
            p.owner = "c";
            await clientWithMock.RegisterNewProperty(p);

            mockBlockService.Verify(m => m.InvokePost(expectedUrl, JsonConvert.SerializeObject(p)));
        }

        [TestMethod]
        public async Task sendProperty_will_invoke_post()
        {
            var expectedUrl = HyperledgerConsts.TransactionUrl;

            Transaction t = new Transaction();
            t.newOwner = "a";
            t.property = "b";
            await clientWithMock.sendProperty(t);

            mockBlockService.Verify(m => m.InvokePost(expectedUrl, JsonConvert.SerializeObject(t)));
        }

        [TestMethod]
        public void JsonConvertCanDeserializeEvenWhenThereAreExtraFields()
        {
            var results = JsonConvert.DeserializeObject<Trader>(TestJsonObjectConsts.Trader1);
            Assert.AreEqual(results.fullName, String.Format("{0} {1}", results.firstName, results.lastName));
        }

        [TestMethod]
        public async Task If_network_is_down_sendProperty_return_false()
        {
            mockBlockService.Setup(m => m.InvokePost(It.IsAny<String>(), It.IsAny<String>()))
                            .ThrowsAsync(new HttpRequestException());

            var results = await clientWithMock.sendProperty(new Transaction());

            Assert.IsFalse(results);
        }

        [TestMethod]
        public async Task If_network_is_down_cant_register_properties()
        {
            mockBlockService.Setup(m => m.InvokePost(It.IsAny<String>(), It.IsAny<String>()))
                            .ThrowsAsync(new HttpRequestException());

            var results = await clientWithMock.RegisterNewProperty(new Property());

            Assert.IsFalse(results);
        }

        [TestMethod]
        public async Task If_network_is_down_cant_register_trader()
        {
            mockBlockService.Setup(m => m.InvokePost(It.IsAny<String>(), It.IsAny<String>()))
                            .ThrowsAsync(new HttpRequestException());

            var results = await clientWithMock.RegisterNewTrader(new Trader());

            Assert.IsFalse(results);
        }

        [TestMethod]
        public async Task If_network_is_down_cant_get_property()
        {
            mockBlockService.Setup(m => m.InvokeGet(It.IsAny<String>()))
                            .ThrowsAsync(new HttpRequestException());
            mockBlockService.Setup(m => m.InvokeGet(HyperledgerConsts.TraderQueryURL(TestJsonObjectConsts.Trader1ID)))
                            .ReturnsAsync(TestJsonObjectConsts.Trader1);

            await clientWithMock.login(TestJsonObjectConsts.Trader1ID, "");
            var results = await clientWithMock.getMyProperties();

            Assert.IsNull(results);
        }

        [TestMethod]
        public async Task Get_User_Transaction()
        {
            mockBlockService.Setup(m => m.InvokeGet(HyperledgerConsts.OrderedTransactionUrl))
                            .ReturnsAsync(TestJsonObjectConsts.listOfTransactions);
            mockBlockService.Setup(m => m.InvokeGet(HyperledgerConsts.TraderQueryURL(TestJsonObjectConsts.Trader1ID)))
                            .ReturnsAsync(TestJsonObjectConsts.Trader1);

            await clientWithMock.login(TestJsonObjectConsts.Trader1ID, "");
            var results = await clientWithMock.GetUserTransactions();

            Assert.AreEqual(TestJsonObjectConsts.Trader1TransactionId, results[0].transactionId);
        }

        [TestMethod]
        public async Task GettingAllTransactionsWillNowInvokeTheTransactionsInOrder()
        {
            mockBlockService.Setup(m => m.InvokeGet(It.IsAny<String>()))
                            .ReturnsAsync("[]");

            await clientWithMock.GetUserTransactions();

            mockBlockService.Verify(m => m.InvokeGet(HyperledgerConsts.OrderedTransactionUrl));
        }

        [TestMethod]
        public async Task GetPropertyTransactionsWillInvokeGetWithPropertyHistoryURL()
        {
            mockBlockService.Setup(m => m.InvokeGet(It.IsAny<String>()))
                            .ReturnsAsync("[]");
            string property = "Property A";

            await clientWithMock.GetPropertyHistory(property);

            mockBlockService.Verify(m => m.InvokeGet(HyperledgerConsts.PropertyHistoryUrl(Uri.EscapeDataString(property))));
        }

        [TestMethod]
        public async Task GetPropertyHistoryWillOnlyReturnTransactionsInvolvingASingleProperty()
        { 
            string propId = "Property A";
            string escapedPropId = Uri.EscapeDataString(propId);
            mockBlockService.Setup(m => m.InvokeGet(It.IsAny<String>()))
                            .ReturnsAsync(TestJsonObjectConsts.listOfTransactions);

            List<Transaction> transactions = await clientWithMock.GetPropertyHistory(propId);

            transactions.ForEach(t => Assert.AreEqual(t.property, String.Format("resource:org.acme.biznet.Property#{0}",escapedPropId)));
        }

        [TestMethod]
        public async Task GetAllTransactionsWillInvokeGet()
        {
            mockBlockService.Setup(m => m.InvokeGet(It.IsAny<String>()))
                            .ReturnsAsync(TestJsonObjectConsts.listOfTransactions);

            await clientWithMock.GetAllTransactions();

            mockBlockService.Verify(m => m.InvokeGet(HyperledgerConsts.OrderedTransactionUrl));
        }
    }
}
