using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Parsers;

namespace TelegramBot.Parsers
{
    public class UpdatePost: BasePost
    {
        public UpdatePost()
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
                    if (child.Name != "p" ||
                        child.Attributes.Count == 1 && child.Attributes[0].Value == "post_date")
                        continue;

                    Text += child.InnerText + "\n\n";
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
