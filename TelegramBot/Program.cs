using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TelegramBot.Other;
using Telegram.Bot;
using Telegram.Bot.Args;
using System.IO;

namespace TelegramBot
{
    class Program
    {
        private static VersionParser VersionParser;
        private static BlogParser BlogParser;
        private static UpdatesParser UpdatesParser;
        private static TelegramBotClient BotClient;

        static void OnNewBlogPost(BlogPost_t post)
        {
            string text = $"<a href=\'{post.Url}\'>{post.Title}</a>\n\n{post.Text}";

            Logger.Log($"New blog post: {post.Title}");
            var msg = BotClient.SendTextMessageAsync(Settings.Current.ChatId, text, Telegram.Bot.Types.Enums.ParseMode.Html).Result;
            //BotClient.PinChatMessageAsync(Settings.Current.ChatId, msg.MessageId);
        }

        static void OnNewUpdatePost(UpdatePost_t post)
        {
            string text = $"<a href=\'{post.Url}\'>{post.Title}</a>\n\n{post.Text}";

            Logger.Log($"New updates post: {post.Title}");
            var msg = BotClient.SendTextMessageAsync(Settings.Current.ChatId, text, Telegram.Bot.Types.Enums.ParseMode.Html).Result;
            //BotClient.PinChatMessageAsync(Settings.Current.ChatId, msg.MessageId);
        }

        static void OnVersionChange(Version_t oldVersion, Version_t newVersion)
        {
            // Please someone fix my english
            string text;
            if (oldVersion.PatchVersion == newVersion.PatchVersion)
                text = $"CS:GO was updated, but version didn't changed. It means that update was pretty small, and may be not announced.\nClient version: {newVersion.ClientVersion}";
            else
                text = $"CS:GO was updated to version {newVersion.PatchVersion} at {newVersion.VersionDate} {newVersion.VersionTime}";

            Logger.Log(text);
            var msg = BotClient.SendTextMessageAsync(Settings.Current.ChatId, text).Result;
            //BotClient.PinChatMessageAsync(Settings.Current.ChatId, msg.MessageId);
        }

        // Main program loop
        static void Run()
        {
            while (true)
            {
                Thread.Sleep(TimeSpan.FromSeconds(Settings.Current.TimerSeconds));

                //VersionParser.QuickTest();
                BlogParser.QuickTest();
                UpdatesParser.QuickTest();

                VersionParser.Parse();
                BlogParser.Parse();
                UpdatesParser.Parse();
            }
        }

        static void Main(string[] args)
        {
            Console.Title = "TelegramBot | By @User344";
            Settings.Load();
            
            AppDomain.CurrentDomain.UnhandledException += new
            UnhandledExceptionEventHandler(UnhandledExceptionHandler);
            
            Logger.Log("Initializing telegram...");
            BotClient = new TelegramBotClient(Settings.Current.BotToken);
            BotClient.OnReceiveError += OnTelegramError;
            BotClient.OnReceiveGeneralError += OnTelegramGeneralError;

            Logger.Log("Initializing parsers...");
            VersionParser = new VersionParser();
            BlogParser = new BlogParser();
            UpdatesParser = new UpdatesParser();

            VersionParser.OnVersionChange += new OnVersionChange(OnVersionChange);
            BlogParser.OnNewBlogPost += new OnNewBlogPost(OnNewBlogPost);
            UpdatesParser.OnNewUpdatePost += new OnNewUpdatePost(OnNewUpdatePost);

            Logger.Log("Done! Successfully started!");
            Run();
        }
        
        private static void OnAppCrash(Exception ex)
        {
            File.WriteAllText("crash.txt", ex.ToString());
        }

        private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            OnAppCrash((e.ExceptionObject as Exception));
        }

        private static void OnTelegramGeneralError(object sender, ReceiveGeneralErrorEventArgs e)
        {
            OnAppCrash(e.Exception);
        }

        private static void OnTelegramError(object sender, ReceiveErrorEventArgs e)
        {
            OnAppCrash(e.ApiRequestException);
        }
    }
}
