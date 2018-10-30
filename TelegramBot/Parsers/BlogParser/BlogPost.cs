using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Parsers;

namespace TelegramBot.Parsers
{
    public class BlogPost: BasePost
    {
        public BlogPost()
        {

        }

        public override bool Parse(HtmlNode node)
        {
            try
            {
                var TitleNode = node.ChildNodes.Where(x => (x.Name == "h2")).First();
                Title = TitleNode.InnerText;
                Url = TitleNode.ChildNodes.Where(x => (x.Name == "a")).First().Attributes.Where(x => (x.Name == "href")).First().Value;
                
                foreach (var child in node.ChildNodes)
                {
                    if (child.Name == "h4")
                    {
                        Text += $"*{child.InnerText}*\n";
                    }
                    if (child.Name == "p")
                    {
                        if (child.Attributes.Count > 0)
                        {
                            if (child.Attributes[0].Value == "post_date")
                                continue;
                        }
                        if (child.ChildNodes.Count > 0)
                        {
                            if (child.ChildNodes[0].Name == "script" ||
                                child.ChildNodes[0].Name == "a")
                                continue;
                        }

                        foreach (var childNode in child.ChildNodes)
                        {
                            if (childNode.Name == "#text")
                            {
                                Text += childNode.InnerText;
                            }
                            else if (childNode.Name == "a")
                            {
                                var hrefList = childNode.Attributes.Where(x => x.Name == "href");
                                if (hrefList.Count() <= 0) continue;
                                
                                Text += $"<a href=\'{hrefList.First().Value}\'>{childNode.InnerText}</a>";
                            }
                        }

                        Text += "\n\n";
                    }
                }

                this.Node = node;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
