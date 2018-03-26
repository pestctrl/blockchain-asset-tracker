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
        Trader trader;
        Transaction transaction;

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
        public async Task TestInvokeGetMethodAsync()
        {
            var results = await hyperledgerService.InvokeGet("/api/org.acme.biznet.Trader/TRADER1");
            trader = JsonConvert.DeserializeObject<Trader>(results);

            Assert.AreEqual("TRADER1", trader.traderId);
        }
        
        String trader1Transaction = "{\"$class\":\"org.acme.biznet.Trade\",\"property\": \"Asset A\", \"newOwner\": \"TRADER1\"}";

        /*
        [TestMethod]
        public async Task a()
        {
            var result = await hyperledgerService.InvokePost("/api/org.acme.biznet.Trade", trader1Transaction);
            transaction = JsonConvert.DeserializeObject<Transaction>(result);

            Assert.AreEqual("TRADER1", transaction.newOwner);
        }
        */
    }
}