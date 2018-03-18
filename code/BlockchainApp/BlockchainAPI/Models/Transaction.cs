using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace BlockchainAPI.Models
{
    public class Transaction
    {
        [JsonProperty("$Class")]
        public string objectType { get; set; }
        public string property { get; set; }
        public string newOwner { get; set; }
        public string transactionId { get; set; }
        public DateTime timestamp { get; set; }
    }
}
