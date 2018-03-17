using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlockchainAPI.Models
{
    public class Property
    {
        [JsonProperty("$class")]
        public string objectType { get; set; }
        public string PropertyId { get; set; }
        public string description { get; set; }
        public string owner { get; set; }
    }
}
