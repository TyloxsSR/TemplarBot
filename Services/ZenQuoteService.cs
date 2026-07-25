using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace TemplarBot.Services
{
    public class ZenQuotesService
    {
        private HttpClient client = new HttpClient();

        public ZenQuotesService()
        {
            client.Timeout = TimeSpan.FromSeconds(5);
        }

        public async Task<ZenQuotes> GetRandomQuote()
        {
            int maxRetries = 3;

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    var responseMessage = await client.GetAsync("https://zenquotes.io/api/random");

                    if (!responseMessage.IsSuccessStatusCode)
                    {
                        await Task.Delay(500);
                        continue;
                    }

                    var json = await responseMessage.Content.ReadAsStringAsync();

                    // ZenQuotes returns an ARRAY: [ { q, a, h } ]
                    var quotes = JsonSerializer.Deserialize<List<ZenQuotes>>(json);

                    if (quotes != null && quotes.Count > 0)
                        return quotes[0]; // return the first quote

                    return null;
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

public class ZenQuotes
{
    public string q { get; set; } // Quote text
    public string a { get; set; } // Author
    public string h { get; set; } // HTML formatted version
}
