﻿using System;
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
            string s = "/api/org.example.biznet.Trader/TRADER1";
        }

        [TestMethod]
        public async Task TestInvokeGetMethod()
        {
            var results = await hyperledgerService.InvokeGet("/api/org.example.biznet.Trader/TRADER1");
            Trader trader = JsonConvert.DeserializeObject<Trader>(results);

            Assert.AreEqual("TRADER1", trader.traderId);
        }

        /*
        [TestMethod]
        public async Task TestInokePostMethod()
        { 
            var testTransaction = "{\"$class\":\"org.example.biznet.Trade\",\"property\": \"Asset CC\", \"newOwner\": \"TRADER1\"}";
            var result = await hyperledgerService.InvokePost("/api/org.example.biznet.Trade", testTransaction);
            Transaction transaction = JsonConvert.DeserializeObject<Transaction>(result);

            Assert.AreEqual("TRADER1", transaction.newOwner);
        }*/
        
    }
}