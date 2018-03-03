using System;
using System.Net.Http;

namespace consoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpClient client = new HttpCleint();
            Console.WriteLine("Hello World!");
            
            // Making a GET request is the easiest.
            // Just call the GetStringAsync method with the correct URL
            var propertiesJson = await client.GetStringAsync("129.213.108.211:3000/api/org.acme.biznet.Property");
            Console.WriteLine(propertiesJson);
        }
    }
}
