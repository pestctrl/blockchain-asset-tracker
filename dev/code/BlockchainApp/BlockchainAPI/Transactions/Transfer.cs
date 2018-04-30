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
        public Double longitude { get; set; }
        public Double latitude { get; set; }
        public string transactionId { get; set; }
        public DateTime? timestamp { get; set; }
    }
}
