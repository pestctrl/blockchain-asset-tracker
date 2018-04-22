using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlockchainAPI.Models;
using Moq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using BlockchainAPI.Transactions;
using System.Linq;

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
        public async Task If_trader_was_not_found_return_false()
        {
            mockBlockService.Setup(m => m.InvokeGet(It.IsAny<String>()))
                            .ThrowsAsync(new HttpRequestException());

            var results = await clientWithMock.UserExists("");

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

            BlockchainClient.Error error = await clientWithMock.sendProperty(new Transaction());

            Assert.AreEqual(error, BlockchainClient.Error.NETWORK);
        }

        [TestMethod]
        public async Task If_network_is_down_cant_register_properties()
        {
            mockBlockService.Setup(m => m.InvokePost(It.IsAny<String>(), It.IsAny<String>()))
                            .ThrowsAsync(new HttpRequestException());

            BlockchainClient.Error error = await clientWithMock.RegisterNewProperty(new Property());

            Assert.AreEqual(error, BlockchainClient.Error.NETWORK);
        }

        [TestMethod]
        public async Task If_network_is_down_cant_register_trader()
        {
            mockBlockService.Setup(m => m.InvokePost(It.IsAny<String>(), It.IsAny<String>()))
                            .ThrowsAsync(new HttpRequestException());

            BlockchainClient.Error error = await clientWithMock.RegisterNewTrader(new Trader());

            Assert.AreEqual(error, BlockchainClient.Error.NETWORK);
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
        public async Task GettingAllTransactionsWillNowInvokeTheTransactionsInOrder()
        {
            mockBlockService.Setup(m => m.InvokeGet(It.IsAny<String>()))
                            .ReturnsAsync("[]");

            await clientWithMock.GetUserTransactions();

            mockBlockService.Verify(m => m.InvokeGet(HyperledgerConsts.OrderedTransactionUrl));
        }

        [TestMethod]
        public async Task GetAllTransactionsWillInvokeGet()
        {
            mockBlockService.Setup(m => m.InvokeGet(It.IsAny<String>()))
                            .ReturnsAsync(TestJsonObjectConsts.listOfTransactions);

            await clientWithMock.GetAllTransactions();

            mockBlockService.Verify(m => m.InvokeGet(HyperledgerConsts.OrderedTransactionUrl));
        }

        [TestMethod]
        public async Task PropertyHistoryWillInvokeANewURL()
        {
            mockBlockService.Setup(m => m.InvokeGet(It.IsAny<String>()))
                            .ReturnsAsync("[]");

            await clientWithMock.GetPropertyHistory("Property A");

            mockBlockService.Verify(m => m.InvokeGet(HyperledgerConsts.PropertyPackageUrl("Property%20A")));
        }

        [TestMethod]
        public async Task PropertyHistoryWillCallPackageHistoryOnEveryReturnedPackage()
        {
            string property = "Property A";
            var packageList = "[{\"PackageId\": \"PackageA\"}, {\"PackageId\": \"PackageB\"}]";
            List<Package> actualList = JsonConvert.DeserializeObject<List<Package>>(packageList);
            mockBlockService.Setup(m => m.InvokeGet(It.IsAny<String>()))
                            .ReturnsAsync("[]");
            mockBlockService.Setup(m => m.InvokeGet(HyperledgerConsts.PropertyPackageUrl(Uri.EscapeDataString(property))))
                            .ReturnsAsync(packageList);

            await clientWithMock.GetPropertyHistory(property);

            foreach(Package p in actualList)
            {
                mockBlockService.Verify(m => m.InvokeGet(HyperledgerConsts.PackageHistoryUrl(p.PackageId)));
            }
        }

        [TestMethod]
        public async Task CreatePackageShouldInvokeTheRightURL()
        {
            CreatePackage p = new CreatePackage();

            await clientWithMock.CreatePackage(p);

            mockBlockService.Verify(m => m.InvokePost(HyperledgerConsts.CreatePackageUrl, JsonConvert.SerializeObject(p)));
        }

        [TestMethod]
        public async Task UnboxPackageShouldInvokeTheRightURL()
        {
            UnboxPackage p = new UnboxPackage();

            await clientWithMock.UnboxPackage(p);

            mockBlockService.Verify(m => m.InvokePost(HyperledgerConsts.UnboxPackageUrl, JsonConvert.SerializeObject(p)));
        }

        [TestMethod]
        public async Task UserExistsShouldInsteadUseHead()
        {
            var username = It.IsAny<String>();
            mockBlockService.Setup(m => m.InvokeHead(It.IsAny<String>(), It.IsAny<String>()))
                            .ReturnsAsync(true);

            await clientWithMock.UserExists(username);

            mockBlockService.Verify(m => m.InvokeHead(HyperledgerConsts.TraderUrl, username));
        }

        [TestMethod]
        public async Task PropertyExistsShouldInsteadUseHead()
        {
            var propertyId = It.IsAny<String>();
            mockBlockService.Setup(m => m.InvokeHead(It.IsAny<String>(), It.IsAny<String>()))
                            .ReturnsAsync(true);

            await clientWithMock.PropertyExists(propertyId);

            mockBlockService.Verify(m => m.InvokeHead(HyperledgerConsts.PropertyUrl, propertyId));
        }

        [TestMethod]
        public async Task TestSortByDate()
        {
            List<Transfer> list = new List<Transfer>
            {
                new Transfer {timestamp = DateTime.Today},
                new Transfer {timestamp = DateTime.Today.AddDays(-1)},
                new Transfer {timestamp = DateTime.Today.AddDays(1)}
            };

            list = list.OrderBy(x => x.timestamp).ToList();

            for(int i = 0; i < list.Count()-1; i++)
            {
                Assert.IsTrue(list[i].timestamp < list[i+1].timestamp);
            }
        }

        [TestMethod]
        public async Task PropertyHistoryWillReturnListOfTransfersInOrderWithMostRecentFirst()
        {
            string property = "Property A";
            string package = "PackageA";
            mockBlockService.Setup(m => m.InvokeGet(HyperledgerConsts.PackageHistoryUrl(Uri.EscapeDataString(package))))
                            .ReturnsAsync("[{\"timestamp\":\"2018-04-21T19:47:54.368Z\"},{\"timestamp\":\"2018-04-21T19:48:54.368Z\"}]");
            mockBlockService.Setup(m => m.InvokeGet(HyperledgerConsts.PropertyPackageUrl(Uri.EscapeDataString(property))))
                            .ReturnsAsync("[{\"PackageId\": \"PackageA\"}]");

            List<Transfer> results = await clientWithMock.GetPropertyHistory(property);

            for (int i = 0; i < results.Count() - 1; i++)
            {
                Assert.IsTrue(results[i].timestamp > results[i + 1].timestamp);
            }
        }

        [TestMethod]
        public async Task TestPropertyHistoryOnMultiplePackages()
        {
            string property = "Property A";
            string package1 = "PackageA";
            string package2 = "PackageB";
            mockBlockService.Setup(m => m.InvokeGet(HyperledgerConsts.PackageHistoryUrl(Uri.EscapeDataString(package1))))
                            .ReturnsAsync("[{\"timestamp\":\"2018-04-21T19:47:54.368Z\"},{\"timestamp\":\"2018-04-21T19:48:54.368Z\"}]");
            mockBlockService.Setup(m => m.InvokeGet(HyperledgerConsts.PackageHistoryUrl(Uri.EscapeDataString(package2))))
                            .ReturnsAsync("[{\"timestamp\":\"2018-04-21T19:46:54.368Z\"},{\"timestamp\":\"2018-04-21T19:48:58.368Z\"}]");
            mockBlockService.Setup(m => m.InvokeGet(HyperledgerConsts.PropertyPackageUrl(Uri.EscapeDataString(property))))
                            .ReturnsAsync("[{\"PackageId\": \"PackageA\"},{\"PackageId\": \"PackageB\"}]");

            List<Transfer> results = await clientWithMock.GetPropertyHistory(property);

            for (int i = 0; i < results.Count() - 1; i++)
            {
                Assert.IsTrue(results[i].timestamp > results[i + 1].timestamp);
            }
        }

        [TestMethod]
        public async Task GetAllTransfersWillBeInReverseOrder()
        {
            mockBlockService.Setup(m => m.InvokeGet(It.IsAny<String>()))
                            .ReturnsAsync("[{\"timestamp\":\"2018-04-21T19:47:54.368Z\"},{\"timestamp\":\"2018-04-21T19:46:54.368Z\"}]");

            var results = await clientWithMock.GetAllTransfers();

            for (int i = 0; i < results.Count() - 1; i++)
            {
                Assert.IsTrue(results[i].timestamp > results[i + 1].timestamp);
            }
        }
    }
}
