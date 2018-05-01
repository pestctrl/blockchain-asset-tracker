using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace consoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            HttpClient client = new HttpClient();
            Console.WriteLine("Hello World!");
            
            // Making a GET request is the easiest.
            // Just call the GetStringAsync method with the correct URL
            string propertiesJson = await client.GetStringAsync("http://129.213.108.211:3000/api/org.acme.biznet.Property");
            Console.WriteLine(propertiesJson);

            // This is how to create a transaction
            // It's very similar to how to create Traders and Properties
            var values = new Dictionary<string, string>
            {
                { "property","Asset A" },
                { "newOwner","TRADER2" }
            };
            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync("", content);
            var responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseString);
        }
    }
}
