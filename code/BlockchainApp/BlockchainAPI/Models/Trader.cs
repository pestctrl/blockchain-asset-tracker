using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlockchainAPI.Models
{
    public class Trader
    {
        [JsonProperty("$class")]
        public string objectType { get; set; }
        public string traderId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
    }
}
