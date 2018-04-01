using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlockchainAPI;
using BlockchainAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlockchainWebApp.Controllers
{
    [Produces("application/json")]
    [Route("api/Transaction")]
    public class TransactionController : Controller
    {
        BlockchainClient client;

        public TransactionController(IBlockchainService service)
        {
            client = new BlockchainClient(service);
        }

        // GET: api/Transaction
        [HttpGet]
        public async Task<IEnumerable<Transaction>> Get()
        {
            return await client.GetAllTransactions();
        }

        // GET: api/Transaction/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet("History/{id}")]
        public async Task<IEnumerable<Transaction>> GetPropertyHistory(string id)
        {
            return await client.GetPropertyHistory(id);
        }
    }
}
