using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Parsers
{
    public class BaseParser
    {
        private WebClient WebClient;
        public BasePost LastPost;
        public string Url;

        public BaseParser()
        {
            WebClient = new WebClient();
            WebClient.Encoding = Encoding.UTF8;
            LastPost = null;
            Url = "";
        }

        public string DownloadWeb(string link)
        {
            try {
                return WebClient.DownloadString(link);
            } catch {
                return "";
            }
        }
        
        public BasePost Parse()
        {
            var content = DownloadWeb(Url);
            if (String.IsNullOrEmpty(content))
                return null;

            return Parse(content);
        }

        protected virtual BasePost Parse(string content)
        {
            return null;
        }
    }
}
