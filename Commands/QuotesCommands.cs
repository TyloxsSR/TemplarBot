using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TemplarBot;
using TemplarBot.Services;
using TemplarBot.SQLDataInsertion;

namespace TemplarBot.Commands
{
    public class QuoteCommands : BaseCommandModule
    {
        private readonly InsertSQLData _sql;

        public QuoteCommands()
        {
            _sql = new InsertSQLData();
        }

        //INSERTS DATA INTO THE DATABASE
        [Command("insert")]
        public async Task InsertQuotes(CommandContext ctx)
        {
            var quotes = _sql.Quotes();

            foreach (var q in quotes)
                await _sql.InsertQuoteAsync(q.Text, q.Source);

            await ctx.Channel.SendMessageAsync("Quotes inserted!");
        }

        //QUOTES THE SAINT FROM THE SQL DATABASE
        //[Command("quotesaint")]
        //public async Task Quote(CommandContext ctx)
        //{
        //    var result = await _sql.GetRandomQuoteAsync();                            FROM SQL DATABASE

        //    string quote = result.Quote;
        //    string source = result.Source;

        //    await ctx.Channel.SendMessageAsync($"*{quote}* — **{source}**");
        //}



        //WIPE FROM THE DATABASE
        [Command("wipe")]
        public async Task WipeQuotes(CommandContext ctx)
        {
            await _sql.WipeQuoteTableAsync();
            await ctx.Channel.SendMessageAsync("QuoteTable has been wiped.");
        }

        //[Command("saintquotes")]
        //public async Task SaintQuotes(CommandContext ctx)
        //{   
        //    var quotes = _sql.SaintQuotes();

        //    foreach (var q in quotes)
        //    {
        //        await ctx.Channel.SendMessageAsync($"*{q.Quote}* - **{q.Source}**");
        //    }
        //}

        [Command("saintquote")]
        public async Task SaintQuote(CommandContext ctx)
        {
            Console.WriteLine("ping");
            var quotes = _sql.SaintQuotes();

            if (quotes.Count == 0)
            {                                                                                                       //FROM .txt FILE
                await ctx.Channel.SendMessageAsync("No quotes found.");
                return;
            }

            var random = new Random();
            var q = quotes[random.Next(quotes.Count)];

            await ctx.Channel.SendMessageAsync($"{q.Quote} - {q.Source}");
            Console.WriteLine("pong");
        }
        
        [Command("Hello")]
        public async Task HelloCommand(CommandContext context)
        {
            await context.Channel.SendMessageAsync($"Hello {context.User.Username}, Have a wonderful and blessed day!");
        }
        [Command("catfact")]
        public async Task CatFact(CommandContext ctx)
        {
            CatFactService catFactService = new CatFactService();
            int number = 3;


            await ctx.Channel.SendMessageAsync("1. Get one Cat Fact");
            await ctx.Channel.SendMessageAsync("2. Get Multiple Cat Facts");
            await ctx.Channel.SendMessageAsync("3. Exit");
            // GetInteractivity() gives access to the Interactivity module,
            // which allows the bot to wait for user input after a command starts.

            var interactivity = ctx.Client.GetInteractivity();
            // WaitForMessageAsync(...) pauses the command until the user sends
            // their next message. The lambda ensures we only accept messages
            // from the same user and same channel.
            var result = await interactivity.WaitForMessageAsync(
            m => m.Author.Id == ctx.User.Id && m.Channel.Id == ctx.Channel.Id // "Is this person the same person who invoked the command? Is this message in the same channel where the command was invoked?"
            );

            if (result.TimedOut)
            {
                await ctx.RespondAsync("You took too long. Try again.");
                return;
            }

            string choice = result.Result.Content;
            switch (choice)
            {
                case "1":
                    var catFacts = await catFactService.GetCatFacts();
                    if (catFacts == null)
                    {
                        await ctx.Channel.SendMessageAsync("I'm sorry, no cat facts were found!");
                    }
                    else
                    {
                        await ctx.Channel.SendMessageAsync($"Cat Fact: {catFacts.fact}");
                    }
                    break;
                case "2":
                    for (int i = 0; i < number; i++)
                    {
                        var catFact = await catFactService.GetCatFacts();
                        if (catFact == null)
                        {
                            await ctx.Channel.SendMessageAsync("I'm sorry, no cat facts were found!");
                        }
                        else
                        {
                            await ctx.Channel.SendMessageAsync($"Cat Fact: {catFact.fact}");
                            await Task.Delay(3500); // This will create a delay of 3.5 seconds before the next cat fact is printed, making it easier to read.
                        }
                    }
                    break;
                case "3":
                    await ctx.Channel.SendMessageAsync("Goodbye!");
                    return;
                default:
                    await ctx.Channel.SendMessageAsync("Invalid choice, please try again.");
                    break;

            }
        }
    }
}