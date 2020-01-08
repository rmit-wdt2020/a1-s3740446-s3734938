using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Assignment1
{
    public static class ConnectionTest
    {
        static readonly HttpClient client = new HttpClient();
    static async Task GetJson()
        {
            var response = await client.GetStringAsync("https://coreteaching01.csit.rmit.edu.au/~e87149/wdt/services/customers/");
            JArray jsonarray = JArray.Parse(response);
            IList<JToken> customers = jsonarray.Children().ToList();
            foreach(JToken customer in customers)
            {
                Console.WriteLine(customer["CustomerID"]);
                Console.WriteLine(customer["Name"]);
            }


        }
        static async Task Main(string[] args)
        {
            await GetJson();
        }
    }
}
