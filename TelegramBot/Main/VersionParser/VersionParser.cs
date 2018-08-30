using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

// Purpose:
// - Parsing current CS:GO version from GameTracker-CSGO github
// TODO:
// -
//
// Created by User344
// 30.08.2018 01:09

namespace TelegramBot
{
    public delegate void OnVersionChange(Version_t oldVersion, Version_t newVersion);
    
    public class VersionParser
    {
        private WebClient WebClient;
        private Version_t CurrentVersion;
        public event OnVersionChange OnVersionChange;

        public VersionParser()
        {
            WebClient = new WebClient();
            WebClient.Encoding = Encoding.UTF8;

            CurrentVersion = null;

            Parse();
        }

        public string RequestWeb(string link)
        {
            return WebClient.DownloadString(link);
        }
        
        public void QuickTest()
        {
            CurrentVersion.ClientVersion = 100;
        }

        public bool Parse()
        {
            const string link = "https://raw.githubusercontent.com/SteamDatabase/GameTracking-CSGO/master/csgo/steam.inf";

            var data = RequestWeb(link);
            if (String.IsNullOrEmpty(data))
                return false;

            var version = new Version_t();
            if (!version.Parse(data))
                return false;

            if (CurrentVersion == null)
                CurrentVersion = version;

            if (version != CurrentVersion)
            {
                OnVersionChange(CurrentVersion, version);
                CurrentVersion = version;
            }

            return true;
        }
    }
}
