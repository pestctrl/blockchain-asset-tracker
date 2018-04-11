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
            var results = await hyperledgerService.InvokeGet("/api/org.example.biznet.Trader/TRADER1");
            Trader trader = JsonConvert.DeserializeObject<Trader>(results);

            Assert.AreEqual("TRADER1", trader.traderId);
        }

        
        [TestMethod]
        public async Task TestInokePostMethod()
        { 
            var traderData = "{\"$class\": \"org.example.biznet.Trader\",\"traderId\": \"testId\",\"firstName\": \"testFirstname\",\"lastName\": \"testLastname\"}";

            var result = await hyperledgerService.InvokePost("/api/org.example.biznet.Trader", traderData);
            Trader trader = JsonConvert.DeserializeObject<Trader>(result);

            Assert.AreEqual("testId", trader.traderId);
        }
        
    }
}