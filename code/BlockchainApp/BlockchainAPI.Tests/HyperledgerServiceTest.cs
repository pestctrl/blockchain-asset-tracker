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
        public async Task getTraderInformationWhenCallingQueryWithTraderID()
        {
            var results = await hyperledgerService.InvokeGet(HyperledgerConsts.TraderQueryURL("TRADER1"));
            Trader trader = JsonConvert.DeserializeObject<Trader>(results);

            Assert.AreEqual("TRADER1", trader.traderId);
        }

        [TestMethod]
        public async Task WhenCallingInvalidUserIDReturnHttpException()
        {
            try
            {
                var results = await hyperledgerService.InvokeGet(HyperledgerConsts.TraderQueryURL("Invalid"));
            }
            catch(HttpRequestException)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public async Task callInvokePostToCreateANewProperty()
        {
            Property property = new Property
            {
                PropertyId = "Property KKK",
                owner = "testOwner",
                description = "test"
            };

            await hyperledgerService.InvokePost(HyperledgerConsts.PropertyUrl, JsonConvert.SerializeObject(property));
            var result = await hyperledgerService.InvokeHead(HyperledgerConsts.PropertyUrl, property.PropertyId);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task CallInValidPropertyWillReturnFalse()
        {
            Property property = new Property
            {
                PropertyId = "InvalidProperty"
            };

            var result = await hyperledgerService.InvokeHead(HyperledgerConsts.PropertyUrl, property.PropertyId);
            
            Assert.IsFalse(result);
        } 

        [TestMethod]
        public async Task loginFailWithNonExistUser()
        {
            var expectResult = "User notExist doesn't exist";
            User user = new User
            {
                username = "notExist",
                password = "notExist"
            };

            var Results = await hyperledgerService.InvokePostAuthentication(FlaskConsts.LoginUrl, JsonConvert.SerializeObject(user));
            var checkUser = JsonConvert.DeserializeObject<messageCredential>(Results);

            Assert.AreEqual(expectResult, checkUser.message);
        }

        [TestMethod]
        public async Task acessTokenIsNullWhenLoginWithInvalidUser()
        {
            User user = new User
            {
                username = "InvalidUser",
                password = "InvalidUser"
            };

            var Results = await hyperledgerService.InvokePostAuthentication(FlaskConsts.LoginUrl, JsonConvert.SerializeObject(user));
            var checkUser = JsonConvert.DeserializeObject<messageCredential>(Results);

            Assert.IsNull(checkUser.access_token);
        }

        [TestMethod]
        public async Task accessTokenisNotNullWhenLoginWithValidUser()
        {
            User user = new User
            {
                username = "TRADER1",
                password = "test"
            };

            var Results = await hyperledgerService.InvokePostAuthentication(FlaskConsts.LoginUrl, JsonConvert.SerializeObject(user));
            var checkUser = JsonConvert.DeserializeObject<messageCredential>(Results);

            Assert.IsNotNull(checkUser.access_token);
        }
    }
}