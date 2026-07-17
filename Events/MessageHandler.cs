using DSharpPlus;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplarBot;
using TemplarBot.Moderation;
using TemplarBot.Services;


namespace TemplarBot.Events
{
    public static class MessageHandler
    {
        public static async Task OnMessageCreated(DiscordClient client, MessageCreateEventArgs messageCreate) // This method is called whenever a new message is created in any channel the bot has access to.
        {
            if (messageCreate.Author.IsBot) // Author has a property called IsBot which is set to a bool. True or False. This checks if the author of the message is a bot. If it is, we return immediately and do nothing. This prevents the bot from reacting to messages sent by other bots (including itself), which can help avoid infinite loops and unnecessary processing.
                return; // This stops the method entirely.

            await ReactionService.HandleReactions(client, messageCreate); // This calls the HandleReactions method from the ReactionService class, passing in the client and the messageCreate event args. This is where you would put any logic related to adding reactions to messages based on their content or other criteria.
            await WordFilter.CheckMessage(messageCreate); // This calls the CheckMessage method from the WordFilter class, passing in the messageCreate event args. This is where you would put any logic related to checking the content of the message for banned words or phrases and taking appropriate action (like deleting the message, warning the user, etc.).
        }
    }
}