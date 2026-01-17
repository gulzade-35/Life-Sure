using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LifeSure.Services
{
    public class RapidApiChatGptService
    {
        private readonly string _apiKey;
        private readonly string _apiHost = "chatgpt-42.p.rapidapi.com";
        private readonly HttpClient _httpClient;

        public RapidApiChatGptService(string apiKey)
        {
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("x-rapidapi-key", _apiKey);
            _httpClient.DefaultRequestHeaders.Add("x-rapidapi-host", _apiHost);
        }

        public async Task<string> SendMessageAsync(string message)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"https://{_apiHost}/chat"),
                Content = new StringContent(
                    JsonConvert.SerializeObject(new
                    {
                        model = "gpt-4o-mini",
                        messages = new[]
                        {
                            new { role = "user", content = message }
                        }
                    }),
                    Encoding.UTF8,
                    "application/json"
                )
            };

            using (var response = await _httpClient.SendAsync(request))
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"API Error: {response.StatusCode} - {responseContent}");
                }

                dynamic result = JsonConvert.DeserializeObject(responseContent);
                string reply = result?.choices?[0]?.message?.content;
                return reply ?? "No response from API";
            }
        }
    }
}