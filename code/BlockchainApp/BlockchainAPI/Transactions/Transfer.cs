using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlockchainAPI.Transactions
{
    public class Transfer
    {
        [JsonProperty("$class")]
        public string objectType { get; set; }
        public string package { get; set; }
        public string origHandler { get; set; }
        public string newHandler { get; set; }
        public int longitude { get; set; }
        public int latitude { get; set; }
        public string transactionId { get; set; }
        public DateTime? timestamp { get; set; }
    }
}
