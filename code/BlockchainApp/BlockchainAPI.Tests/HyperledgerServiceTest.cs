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
    public class HyperledgerServiceTest
    {
        private HyperledgerService hyperledgerService;

        [TestInitialize]
        public void init()
        {
            hyperledgerService = new HyperledgerService();
        }

        [TestMethod]
        public void canaryTest()
        {
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task TestInvokeGetMethod()
        {
            var results = await hyperledgerService.InvokeGet(HyperledgerConsts.TraderQueryURL("TRADER1"));
            Trader trader = JsonConvert.DeserializeObject<Trader>(results);

            Assert.AreEqual("TRADER1", trader.traderId);
        }

        
        [TestMethod]
        public async Task TestInokePostMethod()
        {
            var expectResult = "error";

            var result = await hyperledgerService.InvokePost(HyperledgerConsts.TraderUrl, "Fail");

            Assert.AreEqual(expectResult , result.Substring(2,5));
        }

        [TestMethod]
        public async Task TestInvokeHeadMethod()
        {
            Assert.IsTrue(await hyperledgerService.InvokeHead(HyperledgerConsts.TraderUrl, "TRADER1"));
        }  

        [TestMethod]
        public async Task TestInvokeAuthenticationMethod()
        {
            var expectResult = "Failed to decode JSON object: Expecting value: line 1 column 1 (char 0)";

            var Results = await hyperledgerService.InvokePostAuthentication(FlaskConsts.LoginUrl, "Fail");
            var checkUser = JsonConvert.DeserializeObject<messageCredential>(Results);

            Assert.AreEqual(expectResult, checkUser.message);
        }
    }
}