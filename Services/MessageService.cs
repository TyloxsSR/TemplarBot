using DSharpPlus;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplarBot.Services
{
    
    public class MessageService
    {
        public async Task StartTimedMessages(DiscordClient client)
        {
            await Task.Delay(5000);

            ulong GenChatId = 1103149837505015830;
            var channel = await client.GetChannelAsync(GenChatId);

            var messages = new List<string>
            {
                "You're stronger than you think.",
                "Take a moment to breathe.",
                "Hope you're having a peaceful day.",
                "Small reminder: you're doing great.",
                "Stay hydrated and be kind to yourself.",
                "You've made it this far. What can't you do?",
                "Progress is still progress. Even if slow.",
                "You're capable of more than you know.",
                "Don't underestimate your strength.",
                "I believe in you.",
                "God made you for a reason. Your life is no coincidence.",
                "You are not alone. - Michael Jordan",
                "You're worthy of peace and rest.",
                "Forward.",
                "Focus on the single step in front of you, not the whole staircase.",
                "You're not done yet.",
                "This is a challenging chapter, not your whole story.",
                "You're becoming something unstoppable.",
                "If all you did today was wake up and breathe, I am proud of you.",
                "The future belongs to those who believe in the beauty of their dreams.",
                "Every mistake is just data showing you a better way forward.",
                "Everything will be okay in the end. If it's not okay, it's not the end",
                "Give yourself the exact same grace you so freely offer to others.",
                "Trust the process and give yourself permission to learn as you go.",
            };

            int index = 0;

            while (true)
            {
                try
                {
                    await channel.SendMessageAsync(messages[index]);

                    index++;
                    if (index >= messages.Count)
                        index = 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Timed message error: {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromMinutes(60));
            }
        }

    }

}
