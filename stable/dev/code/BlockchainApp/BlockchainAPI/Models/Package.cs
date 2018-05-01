using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlockchainAPI.Models
{
    public class Package
    {
        [JsonProperty("$class")]
        public string objectType { get; set; }
        public string PackageId { get; set; }
        public bool active { get; set; }
        public string handler { get; set; }
        public string sender { get; set; }
        public string recipient { get; set; }
        public List<string> contents { get; set; }
    }
}
