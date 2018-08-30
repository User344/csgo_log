using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot
{
    public class BlogPost_t
    {
        public string Title;
        public string Text;

        public BlogPost_t()
        {

        }

        public bool Parse(HtmlNode node)
        {
            try
            {
                Title = node.ChildNodes.Where(x => (x.Name == "h2")).First().InnerText;
                foreach (var child in node.ChildNodes)
                {
                    if (child.Name != "p")
                        continue;

                    if (child.Attributes.Count > 0)
                    {
                        if (child.Attributes[0].Value == "post_date")
                            continue;
                    }
                    if (child.ChildNodes.Count > 0)
                    {
                        if (child.ChildNodes[0].Name == "script")
                            continue;
                        if (child.ChildNodes[0].Name == "a")
                            continue;
                    }

                    Text += child.InnerText + "\n\n";
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool operator ==(BlogPost_t rhs, BlogPost_t lhs)
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

        public static bool operator !=(BlogPost_t rhs, BlogPost_t lhs)
        {
            return !(rhs == lhs);
        }
    }
}
