using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Purpose:
// - Logging things to console with colors
// TODO:
// - Log to file
//
// Created by User344
// 30.08.2018 01:05

namespace TelegramBot.Other
{
    public class Logger
    {
        static Logger()
        {

        }

        private static void Log(string prefix, ConsoleColor color, string text)
        {
            Console.ForegroundColor = color;
            foreach (var line in text.Split('\n')) {
                Console.WriteLine($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()} [{prefix}] {line}");
            }
            Console.ResetColor();
        }

        public static void Log(string text)
        {
            Log("LOG", Console.ForegroundColor, text);
        }

        public static void Warn(string text)
        {
            Log("WARNING", ConsoleColor.Yellow, text);
        }

        public static void Error(string text)
        {
            Log("ERROR", ConsoleColor.Red, text);
        }
    }
}
