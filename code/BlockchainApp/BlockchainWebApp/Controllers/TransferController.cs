using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlockchainAPI;
using BlockchainAPI.Models;
using BlockchainAPI.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlockchainWebApp.Controllers
{
    [Produces("application/json")]
    [Route("api/Transfer")]
    public class TransferController : Controller
    {
        BlockchainClient client;

        public TransferController(IBlockchainService service)
        {
            client = new BlockchainClient(service);
        }

        // GET: api/Transaction
        [HttpGet]
        public async Task<IEnumerable<Transfer>> Get()
        {
            return await client.GetAllTransfers();
        }

        // GET: api/Transaction/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet("History/{id}")]
        public async Task<IEnumerable<NewTransfer>> GetPropertyHistory(string id)
        {
            return await client.GetPropertyHistory(id);
        }
    }
}
