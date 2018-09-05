using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

// Purpose:
// - Parsing CS:GO blog posts from official CS:GO blog
// TODO:
// -
//
// Created by User344
// 30.08.2018 01:10

namespace TelegramBot
{
    public delegate void OnNewBlogPost(BlogPost_t post);

    public class BlogParser
    {
        private WebClient WebClient;
        private BlogPost_t CurrentPost;
        public event OnNewBlogPost OnNewBlogPost;

        public BlogParser()
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
            const string link = "http://blog.counter-strike.net";

            var data = RequestWeb(link);
            if (String.IsNullOrEmpty(data))
                return false;

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(data);

            var container = document.GetElementbyId("post_container");
            var node = container.ChildNodes.Where(x => (x.Name == "div")).First();

            var post = new BlogPost_t();
            if (!post.Parse(node))
                return false;

            if (CurrentPost == null)
                CurrentPost = post;

            if (post != CurrentPost)
            {
                OnNewBlogPost(post);
                CurrentPost = post;
            }

            return true;
        }
    }
}
