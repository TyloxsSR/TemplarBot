using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TemplarBot.Commands;
using TemplarBot.Config;
using TemplarBot.Events;
using TemplarBot.Moderation;
using TemplarBot.Services;




namespace TemplarBot.Moderation
{
    public class Program
    {
        // The engine that runs the bot is the DiscordClient. This object is responblible for connecting to Discord,
        // receiving events, and sending messages. It’s the core of your bot.
        private static DiscordClient Client { get; set; }                    // Make both of these fields private and add a setter for better encapsulation and safety
                                                                             // Only this class should be able to touch these. None others.
                                                                             // Example: You don’t want external code calling .Disconnect() or .Dispose() on it.

        // CommandsNextExtension is the module that handles commands and parsing user input.
        // It’s responsible for taking messages, checking if they are commands, and executing the corresponding code.
        private static CommandsNextExtension Commands { get; set; } // Make sure both fields are static as well to avoid multiple connections and duplicated commands.
                                                                    // An instance is just a living object created from a class.// the static keyword ensures that the whole programe shares one client/instance.
                                                                    // This class owns one single Discord client and one single CommandsNext instance, and nobody outside this class is allowed to touch them.

        // Why do we make these two fields instances instead of variables?
        // Because local variables disappear when the method ends and properties stay alive as long as the program runs.

        // --------------------------------------------------------------------------------------------------------------------------------------------------------------------

        // I've noticed the kyaword async alot. async basically means "This method can use await inside it."
        // Discord bots have to use await because connecting to Disocord is asynchronous. It takes time to connect,
        // and you don't want to freeze your whole program while waiting for it.
        // The Task class exists here as a promise that this method will eventually finish.
        // It allows the program to keep running and do other things while waiting for the connection to complete.
        // void cannot be awaited, so we use Task instead to indicate that this method is asynchronous and can be awaited by other code if needed.
        static async Task Main(string[] args)
        {
            var prefix = TokenConfig.Prefix;
            var token = TokenConfig.Token;
            Console.WriteLine($"Prefix loaded: '{prefix}'");
            Console.WriteLine("RUNNING VERSION: 7/12/2026 4:30PM");


            if (string.IsNullOrWhiteSpace(token))
            {
                Console.WriteLine("ERROR: DISCORD_TOKEN is not set.");
                return;
            }

            var discordConfig = new DiscordConfiguration
            {
                Intents = DiscordIntents.All,
                Token = token,
                TokenType = TokenType.Bot,
                AutoReconnect = true
            };



            Client = new DiscordClient(discordConfig);
            Client.Ready += ReadyHandler.Client_Ready;

            var commandsConfig = new CommandsNextConfiguration
            {
                EnableDms = true,
                EnableMentionPrefix = true,
                CaseSensitive = false,
                EnableDefaultHelp = false,
                StringPrefixes = new string[] { prefix }
            };

            Client = new DiscordClient(discordConfig);
            Client.Ready += ReadyHandler.Client_Ready;

            Client.UseInteractivity(new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromSeconds(30)
            });

            Commands = Client.UseCommandsNext(commandsConfig);
            Commands.RegisterCommands<TestCommands>();
            Commands.RegisterCommands<QuoteCommands>();


            Client.MessageCreated += MessageHandler.OnMessageCreated;
            WordFilter.LoadBadWords();

            QuotePosterService quotePoster = new QuotePosterService(Client, 1103149837505015830);
            await quotePoster.StartAsync();

            await Client.ConnectAsync();
            await Task.Delay(-1);


        }

    }
}
