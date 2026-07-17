using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplarBot.Events
{
    public static class ReadyHandler
    {
        public static Task Client_Ready(DiscordClient sender, ReadyEventArgs args)
        {
            // This is the Discord client instance that fires the Ready event!
            // Even though you only have one client, C# events always pass the sender so you know who triggered the event.
            // If you had multiple clients (rare, but possible), this tells you which one became ready. At the moment, it is DSharpPlus.DiscordClient!
            // Why this exact signature? Because every event in C# has a specific “delegate signature” (required method format). In order to function.

            Console.WriteLine("Bot is connected and ready to go! " + "\nSender: " + sender + " \nArgs: " + args); // This prints a message to the console indicating that the bot is ready, along with some information about the sender and event arguments. This is useful for debugging and confirming that the bot has successfully connected to Discord.
            sender.UpdateStatusAsync(new DiscordActivity("Serving the faithful")); // This sets the bot's status message to "Serving the faithful". This is optional but can be a nice touch to let users know what the bot is doing or to add some personality.
            return Task.CompletedTask; // This returns a completed task to satisfy the method's return type. Since this event handler doesn't need to perform any asynchronous operations, we can simply return a completed task to indicate that the event has been handled.

            // This method is called when the bot has successfully connected to Discord and is ready to start receiving events and commands.
            // It’s a good place to put any initialization code that needs to run once the bot is online.
        }
    }
}
