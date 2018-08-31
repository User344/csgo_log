using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot
{
    public class UpdatePost_t
    {
        public string Url;
        public string Title;
        public string Text;

        public UpdatePost_t()
        {

        }

        public bool Parse(HtmlNode node)
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

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool operator ==(UpdatePost_t rhs, UpdatePost_t lhs)
        {
            if (object.ReferenceEquals(lhs, null))
            {
                if (object.ReferenceEquals(rhs, null))
                {
                    return true;
                }

                return false;
            }

            return rhs.Title == lhs.Title;
        }

        public static bool operator !=(UpdatePost_t rhs, UpdatePost_t lhs)
        {
            return !(rhs == lhs);
        }
    }
}
