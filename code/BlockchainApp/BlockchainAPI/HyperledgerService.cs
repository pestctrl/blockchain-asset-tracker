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

        public async Task<bool> InvokeHead(string url, string data)
        {
            var URL = Flurl.Url.Combine(HyperledgerConsts.ipAddress, url, Uri.EscapeDataString(data));
            var results = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, new Uri(URL)));

            return results.IsSuccessStatusCode;
        }

        public async Task<string> InvokePostFlask(string request, String jsonObject)
        {
            var url = Url.Combine(FlaskConsts.IPAddress, Uri.EscapeUriString(request));
            var results = await client.PostAsync(url, new StringContent(jsonObject, Encoding.UTF8, "application/json"));
            return await results.Content.ReadAsStringAsync();
        }

        public async Task<String> InvokeGetFlask(String request)
        {
            var url = Url.Combine(FlaskConsts.IPAddress, request);
            return await client.GetStringAsync(url);
        }
    }
}
