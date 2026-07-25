using System;
using System.Threading.Tasks;
using DSharpPlus;
using TemplarBot.Services;

public class QuotePosterService
{
    private readonly DiscordClient _client;
    private readonly ZenQuotesService _quoteService;
    private readonly ulong _channelId; // where quotes will be posted

    public QuotePosterService(DiscordClient client, ulong channelId)
    {
        _client = client;
        _channelId = channelId;
        _quoteService = new ZenQuotesService();
    }

    public async Task StartAsync()
    {
        _ = Task.Run(async () =>
        {
            while (true)
            {
                try
                {
                    var quote = await _quoteService.GetRandomQuote();

                    if (quote != null)
                    {
                        var channel = await _client.GetChannelAsync(_channelId);

                        await channel.SendMessageAsync(
                            $"**{quote.q}**\n— *{quote.a}*"
                        );
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Quote poster error: " + ex.Message);
                }

                // Wait 3 hours
                await Task.Delay(TimeSpan.FromHours(3));
            }
        });
    }
}
