using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Parsers;

namespace TelegramBot.Managers
{
    public class ParserManager
    {
        private List<BaseParser> Parsers;

        public ParserManager()
        {
            Parsers = new List<BaseParser>
            {
                new BlogParser(),
                new UpdatesParser()
            };
        }
        
        public void Test()
        {
            foreach (var parser in Parsers)
            {
                if (parser.LastPost != null)
                    parser.LastPost.Title = "UNDEFINED";
            }
        }

        public bool Run()
        {
            foreach (var parser in Parsers)
            {
                var oldPost = parser.LastPost;
                var newPost = parser.Parse();

                if (oldPost == null)
                    oldPost = newPost;

                if (!oldPost.Equals(newPost))
                    Program.NotifyManager.Notify(newPost);

                parser.LastPost = newPost;
            }

            return true;
        }
    }
}
