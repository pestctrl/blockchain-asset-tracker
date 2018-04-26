using System;
using System.Collections.Generic;
using System.Text;

namespace BlockchainAPI.Models
{
    public class messageCredential
    {
        public string message { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }
    }
}
