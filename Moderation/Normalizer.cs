using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplarBot.Moderation
{
    public static class Normalizer
    {
        public static string Normalize(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            input = input.ToLower();

            // Replace leetspeak BEFORE stripping symbols
            input = input
                .Replace("4", "a")
                .Replace("@", "a")
                .Replace("3", "e")
                .Replace("1", "i")
                .Replace("!", "i")
                .Replace("0", "o")
                .Replace("$", "s");

            // Remove spaces
            input = input.Replace(" ", "");

            // Remove punctuation / symbols
            input = new string(input.Where(char.IsLetterOrDigit).ToArray());

            return input;
        }
    }
}