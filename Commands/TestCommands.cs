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
    public class TestCommands : BaseCommandModule
    {
        [Command("Test")] // This is the command that users will type to trigger this method
        public async Task Test(CommandContext context) // This is the method that will be called when the command is triggered. It takes a CommandContext as a parameter, which contains information about the command invocation (like the channel, user, etc.)
        {
            await context.Channel.SendMessageAsync("Test successful!"); // This sends a message back to the channel where the command was invoked, confirming that the command worked.
        }


        [Command("Add")]
        public async Task Add(CommandContext context, params string[] args)
        //public async Task Add(CommandContext context, string _number, string _number2) - DOES NOT WORK
        {

            // If the user didn't provide EXACTLY 2 arguments
            if (args.Length != 2) //
            {
                await context.Channel.SendMessageAsync("You must provide exactly two numbers for this to work silly.");
                await context.Channel.SendMessageAsync("Amount of words used: " + args.Length.ToString());
                return;
            }

            // Try to parse both arguments
            bool isValidFirstNumber = BigInteger.TryParse(args[0], out BigInteger number1);  // Translated: "Try to convert the first command‑line argument into a BigInteger. If successful, store the result in and set to true."
            bool isValidSecondNumber = BigInteger.TryParse(args[1], out BigInteger number2);

            if (!isValidFirstNumber || !isValidSecondNumber) // If either of the arguments failed to parse as a number
            {
                await context.Channel.SendMessageAsync("You cannot pass words here, chief.");
                await context.Channel.SendMessageAsync("Amount of words used: " + "***" + args.Length.ToString() + "***");
                return;
            }

            // Safe to add
            BigInteger result = number1 + number2;
            await context.Channel.SendMessageAsync(result.ToString());
        }


        [Command("Subtract")]
        public async Task Subtract(CommandContext context, int number, int number2)
        {
            try
            {
                int result = number - number2;
                await context.Channel.SendMessageAsync(result.ToString());
            }
            catch (Exception ex)
            {
                await context.Channel.SendMessageAsync("An error occurred: " + ex.Message);
            }
        }

        [Command("Multiply")]
        public async Task Multiply(CommandContext context, int number, int number2)
        {
            try
            {
                int result = number * number2;
                await context.Channel.SendMessageAsync(result.ToString());
            }
            catch (Exception ex)
            {
                await context.Channel.SendMessageAsync("An error occurred: " + ex.Message);
            }
        }

        [Command("Divide")]
        public async Task Divide(CommandContext context, int number, int number2)
        {
            try
            {
                int result = number / number2;
                await context.Channel.SendMessageAsync(result.ToString());
            }
            catch (DivideByZeroException ex)
            {
                await context.Channel.SendMessageAsync("Cannot divide by zero!");
            }
            catch (Exception ex)
            {
                await context.Channel.SendMessageAsync("An error occurred: " + ex.Message);
            }
        }
        [Command("RandomCommand")]
        public async Task RandomCommand(CommandContext context, [RemainingText] string input) // This allows the user to type anything after the command, and it will all be stored in the variable "input" as a single string. So if they type "!RandomCommand Hello World", then input will be "Hello World". If they just type "!RandomCommand" with nothing after it, then input will be an empty string.
        {
            if (string.IsNullOrWhiteSpace(input)) // If they typed NOTHING after the command
            {
                await context.Channel.SendMessageAsync("Try again, buster.");
            }
            else
            {
                await context.Channel.SendMessageAsync($"You just called the random command, {context.User.Username}!");
            }
        }
        [Command("SomeCommand")]
        public async Task SomeCommand(CommandContext ctx, [RemainingText] string input)
        {
            // If they typed NOTHING or
            // If they typed ANYTHING after the command
            if (!string.IsNullOrWhiteSpace(input))
            {
                await ctx.Channel.SendMessageAsync("You cannot type words or numbers with this command.");
                return;
            }

            // If they typed NOTHING (just the command)
            await ctx.Channel.SendMessageAsync($"You just called the random command, {ctx.User.Username}!");

        }
    }
}