using BlockchainAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BlockchainAPI.Tests
{
    [TestClass]
    public class FlaskAuthenticationTest
    {
        FlaskAuthentication mockAuthentication;
        Mock<IBlockchainService> mockBlockService;
        [TestInitialize()]
        public void Mocksetup()
        {

        }
    }
}
