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

        //private static void WaitForCommands(BotMain botMain)
        //{
        //    Console.WriteLine("==============================");
        //    Console.WriteLine("Available bot commands : start, stop, exit");
        //    Console.WriteLine("==============================");

        //    var command = "start";
        //    while (true)
        //    {
        //        switch (command)
        //        {
        //            case "stop":
        //                Console.WriteLine("Bot stopping....");
        //                botMain.Stop();
        //                Console.WriteLine("Bot stopped");
        //                Console.WriteLine("==============================");
        //                break;

        //            case "start":
        //                Console.WriteLine("Bot starting....");
        //                Console.WriteLine("Bot started");
        //                Console.WriteLine("==============================");
        //                break;

        //            case "exit":
        //                return;

        //            default:
        //            {
        //                Console.WriteLine($"{command} is not a valid command");
        //                break;
        //            }
        //        }

        //        Console.Write("Bot:");
        //        command = Console.ReadLine();
        //    }
        //}

    }
}
