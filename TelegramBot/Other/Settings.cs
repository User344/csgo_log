using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Purpose:
// - Loading and storing settings from file
// TODO:
// - Save to/Load from file
//
// Created by User344
// 30.08.2018 01:06

namespace TelegramBot.Other
{
    public class Settings_
    {
        public double TimerSeconds;
        public string BotToken;
        public string ChatId;
    }

    public class Settings
    {
        public static Settings_ Current;

        static Settings()
        {

        }

        public static bool Load()
        {
            Settings.Current = new Settings_();
            Settings.Current.TimerSeconds = 10;
            Settings.Current.BotToken = "";
            Settings.Current.ChatId = "@csgo_log";

            return false;
        }
    }
}
