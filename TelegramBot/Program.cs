using System;
using System.IO;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Args;
using TelegramBot.Managers;
using TelegramBot.Other;
using Telegraph.Net;

namespace TelegramBot
{
    class Program
    {
        public static CrashManager CrashManager;
        public static NotifyManager NotifyManager;
        public static ParserManager ParserManager;
        
        // Main program loop
        static void Run()
        {
            while (true)
            {
#if DEBUG
                ParserManager.Test();
#endif

                ParserManager.Run();

                Thread.Sleep(TimeSpan.FromSeconds(Settings.Current.TimerSeconds));
            }
        }

        static void Main(string[] args)
        {
            Console.Title = "@csgo_log bot";
            Logger.Log("Initializing...");
            Settings.Load();

            NotifyManager = new NotifyManager();
            ParserManager = new ParserManager();
            CrashManager = new CrashManager();

            Logger.Log("Done! Successfully started!");
            Run();
        }
    }
}
