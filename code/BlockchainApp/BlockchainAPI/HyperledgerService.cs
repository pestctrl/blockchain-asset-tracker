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

        public async Task<string> InvokePost(string request, String jsonObject)
        {
            var url = Url.Combine(HyperledgerConsts.ipAddress, Uri.EscapeUriString(request));
            var results = await client.PostAsync(url, new StringContent(jsonObject, Encoding.UTF8, "application/json"));
            return await results.Content.ReadAsStringAsync();
        }
    }
}
