using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace LifeSure.Services
{
    public class InstagramService
    {
        private readonly string apiKey = ConfigurationManager.AppSettings["RapidApi_Key"];
        private readonly string apiHost = ConfigurationManager.AppSettings["InstagramApi_Host"];
        public async Task<int> GetInstagramFollowerAsync(string username)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-RapidApi-Key", apiKey);
                client.DefaultRequestHeaders.Add("X-RapidApi-Host", apiHost);

                var response = await client.GetAsync($"https://{apiHost}/getprofile/{username}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var json = JObject.Parse(jsonString);

                    return json["followers"]?.Value<int>() ?? 0;
                }
                return 0;
            }
        }
    }
}