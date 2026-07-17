using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace TemplarBot.Moderation

{
    public static class WordFilter
    {
        private static HashSet<string> BannedWords; // This will hold the normalized versions of the bad words for quick lookup. We use a HashSet for O(1) average case lookup time.
        private static List<string> SimpleBadWords; // This will hold the bad words that require simple substring matching.
        private static Regex BadWordRegex; // This will hold the compiled regex for detecting bad words.
        public static void LoadBadWords() // This method will load the bad words from the file and prepare the data structures for detection.
        {

            var path = Path.Combine(AppContext.BaseDirectory, "Resources", "en.txt"); // This constructs the file path to the "en.txt" file located in the "Resources" folder relative to the application's base directory. This is where we expect to find our list of bad words.

            var words = File.ReadAllLines(path) // This reads all lines from the specified file and returns them as an array of strings. Each line in the file is expected to contain one bad word.
                .Where(line => !string.IsNullOrWhiteSpace(line)) // This filters out any lines that are empty or contain only whitespace, ensuring that we only process valid words. "Where line is NOT null or white space."
                .Select(line => line.Trim().ToLower()) // This trims any leading or trailing whitespace from each line and converts it to lowercase. This normalization step helps ensure that our bad word detection is case-insensitive and doesn't get tripped up by accidental spaces.
                .ToList(); // Finally, we convert the resulting sequence of cleaned-up words into a List<string> for easier manipulation later on.

            // Only build regex patterns for alphabetic words
            var regexWords = words // This starts a new sequence based on the original list of words. We will filter and transform this list to create regex patterns for the words that are purely alphabetic.
                .Where(w => Regex.IsMatch(w, @"^[a-z]+$")) // This filters the list to include only those words that consist entirely of lowercase letters (a-z). The regex pattern "^[a-z]+$" means "start of string,
                                                           // followed by one or more lowercase letters, followed by end of string". This ensures that we only create regex patterns for words that are simple and don't contain any special characters, numbers, or symbols, which might be better handled with simple substring matching.
                                                           // Regex breakdown for "^[a-z]+$":

                                                           // ^        = start of the string
                                                           // [a-z]    = any lowercase letter a–z
                                                           // +        = one or more of the previous thing
                                                           // $        = end of the string
                                                           //
                                                           // Together: "match a string made ONLY of lowercase letters, with no extra characters."
                                                           // Examples that match: "hello", "cat"
                                                           // Examples that fail: "Hello", "cat123", "hi!"
                .Select(BuildRegexPattern); // This takes each of the filtered words and applies the BuildRegexPattern function to it, which transforms the word into a regex pattern that can detect leetspeak variations and repeated letters. This allows us to catch attempts to bypass the filter by using common substitutions or letter repetitions.

            // Everything else gets simple substring matching
            SimpleBadWords = words // This starts a new sequence based on the original list of words. We will filter this list to include only those words that are not purely alphabetic, meaning they contain numbers, symbols, or other characters.
                .Where(w => !Regex.IsMatch(w, @"^[a-z]+$")) // This filters the list to include only those words that do NOT consist entirely of lowercase letters ("!"). The regex pattern "^[a-z]+$" matches words that are purely alphabetic, so by negating it with "!", we get all the words that contain something other than just lowercase letters. These might be words that include numbers, symbols, or other characters that don't fit the simple regex pattern we use for alphabetic words.
                .ToList(); // Finally, we convert the resulting sequence of non-alphabetic words into a List<string> for easier manipulation later on. These words will be checked using simple substring matching instead of regex, since they may not fit well into a regex pattern.

            var finalPattern = string.Join("|", regexWords);
            Console.WriteLine("Final regex: " + finalPattern);

            BadWordRegex = new Regex(finalPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }
        private static string BuildRegexPattern(string word)
        {
            // Map letters to common symbol variants
            var map = new Dictionary<char, string>
            {
                { 'a', "[a@4]+" },
                { 'b', "[b8]+" },
                { 'c', "[c(]+" },
                { 'e', "[e3]+" },
                { 'g', "[g69]+" },
                { 'i', "[i1!]+" },          
                { 'l', "[l1]+" },
                { 'o', "[o0]+" },
                { 's', "[s$5]+" },
                { 't', "[t7]+" },
                { 'z', "[z2]+" }
            };

            var parts = word.ToCharArray().Select(c =>
            {
                if (map.ContainsKey(c))
                    return map[c]; // leet-aware pattern
                else
                    return $"{c}+"; // normal repeated-letter pattern
            });

            var core = string.Join(@"[\W_]*", parts);

            // Custom boundaries: not preceded or followed by letters
            return $@"(?<![a-zA-Z]){core}(?![a-zA-Z])";



        }



        //public static void LoadBadWords()
        //{
        //    var path = Path.Combine(AppContext.BaseDirectory, "Resources", "en.txt");

        //    var words = File.ReadAllLines(path)
        //        .Where(line => !string.IsNullOrWhiteSpace(line))
        //        .Select(line => line.Trim().ToLower())
        //        .ToList();

        //    BannedWords = System.IO.File.ReadAllLines(path)
        //        .Where(line => !string.IsNullOrWhiteSpace(line))
        //        .Select(line => Normalizer.Normalize(line.Trim().ToLower()))
        //        .Where(word => !string.IsNullOrWhiteSpace(word))
        //        .Where(word => word.Length >= 4)
        //        .ToHashSet();
        //}
        public static async Task CheckMessage(MessageCreateEventArgs messageCreate)
        {
            var content = messageCreate.Message.Content.ToLower();

            // Regex-based detection
            if (BadWordRegex != null && BadWordRegex.IsMatch(content))
            {
                await messageCreate.Message.DeleteAsync();
                return;
            }

            // Simple substring detection for weird entries
            foreach (var bad in SimpleBadWords)
            {
                if (content.Contains(bad))
                {
                    await messageCreate.Message.DeleteAsync();
                    return;
                }
            }
        }

    }
}
        //public static async Task CheckMessage(MessageCreateEventArgs messageCreate)
        //{


        //    var rawWords = Regex.Split(messageCreate.Message.Content, @"\W+")
        //                        .Where(w => !string.IsNullOrWhiteSpace(w));

        //    foreach (var raw in rawWords)
        //    {
        //        string normalizedWord = Normalizer.Normalize(raw);

        //        Console.WriteLine($"Word: '{raw}' → Normalized: '{normalizedWord}'");

        //        if (BannedWords.Contains(normalizedWord))
        //        {
        //            await messageCreate.Message.DeleteAsync();
        //            return;
        //        }
        //    }
        //}
//    }
//}