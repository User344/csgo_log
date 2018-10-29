using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using TelegramBot.Other;

namespace TelegramBot.Managers
{
    public class CrashManager
    {
        public CrashManager()
        {
            AppDomain.CurrentDomain.UnhandledException += new 
                UnhandledExceptionEventHandler(UnhandledExceptionHandler);
            
            Program.NotifyManager.TelegramClient.OnReceiveError += OnTelegramError;
            Program.NotifyManager.TelegramClient.OnReceiveGeneralError += OnTelegramGeneralError;
        }

        private void OnAppCrash(Exception ex, bool isFatal)
        {
            //string error = $"Exception occurs in unhandled expection handler!\n" +
            //               $"Module: {ex.TargetSite.Module}\n" +
            //               $"Method: {ex.TargetSite.ReflectedType.FullName}\n" +
            //               $"Stack: {ex.StackTrace}";
            string error = ex.ToString();

            if (isFatal)
            {
                File.WriteAllText("crash.txt", ex.ToString());
            }
            else
            {
                Logger.Error(error);
            }
        }

        private void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            OnAppCrash((e.ExceptionObject as Exception), e.IsTerminating);
        }

        private void OnTelegramGeneralError(object sender, ReceiveGeneralErrorEventArgs e)
        {
            OnAppCrash(e.Exception, false);
        }

        private void OnTelegramError(object sender, ReceiveErrorEventArgs e)
        {
            OnAppCrash(e.ApiRequestException, false);
        }
    }
}
