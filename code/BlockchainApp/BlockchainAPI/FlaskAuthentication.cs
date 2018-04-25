using BlockchainAPI.Models;
using BlockchainAPI.Transactions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlockchainAPI
{
    public class FlaskAuthentication
    {
        public enum Error { SUCCESS, FAILED, NETWORK, EXISTS }

        IBlockchainService service;
        public IBlockchainService blockchainService;
        public FlaskAuthentication(IBlockchainService service)
        {
            this.service = service;
        }

        /*public async Task<Error> FlaskRegister(FlaskUser t)
        {
            try
            {
                await blockchainService.InvokePostFlask(FlaskConsts.RegistrationUrl, JsonConvert.SerializeObject(t));
                return Error.SUCCESS;
            }
            catch (HttpRequestException e)
            {
                return Error.NETWORK;
            }
        }*/


    }
}
