using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TemplarBot.Services
{
    public class CatFactService
    {
        private HttpClient client = new HttpClient();
        public CatFactService()
        {
            client.Timeout = TimeSpan.FromSeconds(5);
        }

        public async Task<CatFacts> GetCatFacts() // Removed nullable reference type for compatibility with C# 7.3
        {
            int maxRetries = 3;

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    var responseMessage = await client.GetAsync("https://catfact.ninja/fact");
                    if (!responseMessage.IsSuccessStatusCode)
                    {
                        await Task.Delay(500);
                        continue;
                    }
                    var json = await responseMessage.Content.ReadAsStringAsync();
                    var response = System.Text.Json.JsonSerializer.Deserialize<CatFacts>(json);
                    return response;
                }
                catch
                {
                    if (attempt == maxRetries)
                        return null;
                }
            }
            return null;
        }
    }
}

public class CatFacts
{
    public string fact { get; set; }
    public int length { get; set; }
}