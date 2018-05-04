using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlockchainAPI.Transactions
{
    public class CreatePackage
    {
        [JsonProperty("$class")]
        public string objectType { get; set; }

        // Required
        public string sender { get; set; }
        public string recipient { get; set; }
        public List<string> contents { get; set; }
        public string packageId { get; set; }
        
        public string transactionId { get; set; }
        public DateTime? timestamp { get; set; }
    }
}
