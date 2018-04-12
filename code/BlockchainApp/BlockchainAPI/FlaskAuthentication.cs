using BlockchainAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Text;

namespace BlockchainAPI
{
    public class FlaskAuthentication
    {
        IBlockchainService service; 

        public FlaskAuthentication(IBlockchainService service)
        {
            this.service = service;
        }


    }
}
