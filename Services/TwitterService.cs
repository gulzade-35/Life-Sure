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
    public class TwitterService
    {
        private readonly string apiKey = ConfigurationManager.AppSettings["RapidApi_Key"];
        private readonly string apiHost = ConfigurationManager.AppSettings["TwitterApi_Host"];
        public async Task<int> GetTwitterFollowerAsync(string username)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-RapidApi-Key", apiKey);
                client.DefaultRequestHeaders.Add("X-RapidApi-Host", apiHost);

                var response = await client.GetAsync($"https://{apiHost}/{username}/profile");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var json = JObject.Parse(jsonString);

                    return json["followersCount"]?.Value<int>() ?? 0;
                }
                return 0;
            }
        }
    }
}