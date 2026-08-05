using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TemplarBot.Services
{
    public static class ReactionService
    {
        public static async Task HandleReactions(DiscordClient client, MessageCreateEventArgs messageCreate) // This method is called whenever a new message is created in any channel the bot has access to.
        {
            var content = messageCreate.Message.Content.ToLower();

            // GIGACHAD REACTION FOR TYLER
            ulong tylerId = 1412113070083538944; // <-- replace with your actual Discord user ID

            if (messageCreate.Message.Author.Id == tylerId)
            {
                var giga = DiscordEmoji.FromGuildEmote(client, 1446989032474153060); // <-- your gigachad emoji ID
                await messageCreate.Message.CreateReactionAsync(giga);
            }


            if (messageCreate.Message.Content.IndexOf("bread", StringComparison.OrdinalIgnoreCase) >= 0) // This checks if the message contains the word "bread" in a case-insensitive manner. If it does, we proceed to add a bread reaction to the message. If it's -1 or less, it means "bread" was not found in the message. If it is 0 or greater, it means "bread" was found at that index in the message content.
            {
                await messageCreate.Message.CreateReactionAsync(DiscordEmoji.FromName(client, ":bread:")); // This adds a reaction to the message using the bread emoji. The CreateReactionAsync method is used to add a reaction to a message, and we specify the emoji we want to use by creating it from its name (":bread:"). This will cause the bot to react with a bread emoji whenever someone mentions "bread" in their message, no matter it's index.
            }
            else if (messageCreate.Message.Content.IndexOf("amen", StringComparison.OrdinalIgnoreCase) >= 0) // If the user says "amen"
            {
                await messageCreate.Message.CreateReactionAsync(DiscordEmoji.FromName(client, ":pray:")); // The bot reacts with the praying emoji.
            }
            else if (messageCreate.Message.Content.IndexOf("pentecost", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                await messageCreate.Message.CreateReactionAsync(DiscordEmoji.FromName(client, ":fire:"));
            }
            else if (messageCreate.Message.Content.IndexOf("ben", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                await messageCreate.Message.CreateReactionAsync(DiscordEmoji.FromName(client, ":dog:"));
            }
            // Does the text match this pattern? The @ means "match this pattern". The \b means "word boundary", so we are looking for the word "saint", "st", or "st." as a whole word. The | means "or", so we are checking for any of these three variations. The Regex.IsMatch method returns true if the content matches the pattern, and false otherwise. This allows us to react to messages that mention saints in various ways, while avoiding false positives from words that contain "st" as part of another word (like "stop" or "castle").
            else if (Regex.IsMatch(content, @"\bsaint\b|\bst\b|\bst\.\b")) // This checks if the message contains the word "saint", "st", or "st." as a whole word (not part of another word) in a case-insensitive manner. The \b in the regex pattern ensures that we are matching whole words only. If any of these words are found, we proceed to add an angel reaction to the message.
            {
                await messageCreate.Message.CreateReactionAsync(
                    DiscordEmoji.FromName(client, ":angel:")
                );
                return;
            }
            else if (Regex.IsMatch(content, @"\bpenance\b|\bconfession\b|\brepent\b"))
            {
                await messageCreate.Message.CreateReactionAsync(
                    DiscordEmoji.FromName(client, ":place_of_worship:")
                );
                return;
            }


        }
    }
}
