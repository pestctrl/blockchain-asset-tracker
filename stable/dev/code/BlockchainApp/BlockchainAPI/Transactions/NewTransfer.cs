using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlockchainAPI.Transactions
{
    public class NewTransfer
    {
        [JsonProperty("$class")]
        public string objectType { get; set; }

        public string package { get; set; }
        public string handler { get; set; }
        public bool ingress { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }

        public string transactionId { get; set; }
        public DateTime timestamp { get; set; }
    }
}
