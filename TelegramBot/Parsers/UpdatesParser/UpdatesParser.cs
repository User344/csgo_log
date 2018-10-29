using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Parsers;

// Purpose:
// - Parsing CS:GO blog posts from official CS:GO update log
// TODO:
// -
//
// Created by User344
// 30.08.2018 01:10

namespace TelegramBot.Parsers
{
    public class UpdatesParser: BaseParser
    {
        public UpdatesParser() : base()
        {
            Url = "http://blog.counter-strike.net/index.php/category/updates/";
        }

        protected override BasePost Parse(string content)
        {
            HtmlDocument document = new HtmlDocument();
            try {
                document.LoadHtml(content);
            } catch {
                return null;
            }

            var container = document.GetElementbyId("post_container");
            var node = container.ChildNodes.Where(x => (x.Name == "div")).First();

            var post = new UpdatePost();
            if (!post.Parse(node))
                return null;
            
            return post;
        }
    }
}
