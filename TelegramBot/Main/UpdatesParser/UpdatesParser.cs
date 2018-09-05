using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

// Purpose:
// - Parsing CS:GO blog posts from official CS:GO update log
// TODO:
// -
//
// Created by User344
// 30.08.2018 01:10

namespace TelegramBot
{
    public delegate void OnNewUpdatePost(UpdatePost_t post);

    public class UpdatesParser
    {
        private WebClient WebClient;
        private UpdatePost_t CurrentPost;
        public event OnNewUpdatePost OnNewUpdatePost;

        public UpdatesParser()
        {
            WebClient = null;

            CurrentPost = null;

            Parse();
        }

        public string RequestWeb(string link)
        {
            try {
                WebClient = new WebClient();
                var data = WebClient.DownloadString(link);
                WebClient.Dispose();
                WebClient = null;
                return data;
            } catch {
                return "";
            }
        }

        public void QuickTest()
        {
            CurrentPost.Title = "UNDEFINED";
        }

        public bool Parse()
        {
            const string link = "http://blog.counter-strike.net/index.php/category/updates/";

            var data = RequestWeb(link);
            if (String.IsNullOrEmpty(data))
                return false;

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(data);

            var container = document.GetElementbyId("post_container");
            var node = container.ChildNodes.Where(x => (x.Name == "div")).First();

            var post = new UpdatePost_t();
            if (!post.Parse(node))
            {
                document = null;
                return false;
            }

            if (CurrentPost == null)
                CurrentPost = post;

            if (post != CurrentPost)
            {
                OnNewUpdatePost(post);
                CurrentPost = post;
            }

            document = null;
            return true;
        }
    }
}
