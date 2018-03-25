using Flurl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainAPI
{
    public class HyperledgerService : IBlockchainService
    {
        HttpClient client;

        public HyperledgerService()
        {
            client = new HttpClient();
        }
        
        public async Task<String> InvokeGet(String request)
        {
            var url = Url.Combine(HyperledgerConsts.ipAddress, request);
            return await client.GetStringAsync(url);
        }

        public async Task<string> InvokePost(string request, Dictionary<string, string> parameters)
        {
            var url = Url.Combine(HyperledgerConsts.ipAddress, request);
            var results = await client.PostAsync(url, new FormUrlEncodedContent(parameters));
            return await results.Content.ReadAsStringAsync();
        }
    }
}
