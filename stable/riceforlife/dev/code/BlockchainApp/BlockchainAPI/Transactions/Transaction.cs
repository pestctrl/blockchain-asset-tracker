using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace BlockchainAPI.Transactions
{
    public class Transaction
    {
        [JsonProperty("$class")]
        public string objectType { get; set; }
        public string property { get; set; }
        public string origOwner { get; set; }
        public string newOwner { get; set; }
        public Double latitude { get; set; }
        public Double longitude { get; set; }
        public string transactionId { get; set; }
        public DateTime? timestamp { get; set; }
    }
}
