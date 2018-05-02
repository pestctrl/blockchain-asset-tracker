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
            string url = Url.Combine(HyperledgerConsts.ipAddress, request);

            return await client.GetStringAsync(url);
        }


        public async Task<string> InvokePost(string request, String jsonObject)
        {
            string url = Url.Combine(HyperledgerConsts.ipAddress, Uri.EscapeUriString(request));
            HttpResponseMessage results = await client.PostAsync(url, new StringContent(jsonObject, Encoding.UTF8, "application/json"));

            return await results.Content.ReadAsStringAsync();
        }

        public async Task<bool> InvokeHead(string url, string data)
        {
            string URL = Flurl.Url.Combine(HyperledgerConsts.ipAddress, url, Uri.EscapeDataString(data));
            HttpResponseMessage results = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, new Uri(URL)));

            return results.IsSuccessStatusCode;
        }

        public async Task<string> InvokePostAuthentication(string request, String jsonObject)
        {
            string url = Url.Combine(FlaskConsts.IPAddress, Uri.EscapeUriString(request));
            HttpResponseMessage results = await client.PostAsync(url, new StringContent(jsonObject, Encoding.UTF8, "application/json"));

            return await results.Content.ReadAsStringAsync();
        }
    }
}
