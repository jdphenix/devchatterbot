using System;
using DevChatter.Bot.Core;
using DevChatter.Bot.Infra.Twitch;
using DevChatter.Bot.Startup;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace DevChatter.Bot.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BotInit();
            BuildWebHost(args).Run();
        }

        private static void BotInit()
        {
            Console.WriteLine("Initializing the Bot...");

            (string connectionString, TwitchClientSettings clientSettings) = SetUpConfig.InitializeConfiguration();

            BotMain botMain = SetUpBot.NewBot(clientSettings, connectionString);
            botMain.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
