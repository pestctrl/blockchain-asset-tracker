using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlockchainAPI.Transactions
{
    class UnboxPackage
    {
        [JsonProperty("$class")]
        public string objectType { get; set; }
        public string package { get; set; }
        public string recipient { get; set; }
        public string transactionId { get; set; }
        public DateTime timestamp { get; set; }
    }
}
