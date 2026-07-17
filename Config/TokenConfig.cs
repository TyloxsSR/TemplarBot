using System;


namespace TemplarBot.Config
{
    public static class TokenConfig
    {
        public static string Prefix => Environment.GetEnvironmentVariable("PREFIX");
        public static string Token => Environment.GetEnvironmentVariable("DISCORD_TOKEN");
    }
}