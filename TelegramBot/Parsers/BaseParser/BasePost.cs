using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegraph.Net.Models;

namespace TelegramBot.Parsers
{
    public class BasePost: IEquatable<BasePost>
    {
        public string Title;
        public string Text;
        public string Url;
        public HtmlNode Node;
        
        public string FormatTitle()
        {
            return $"[{this.Title}]({this.Url})";
        }

        public string FormatText()
        {
            return System.Web.HttpUtility.HtmlDecode(this.Text);
        }

        public NodeElement[] buildNode()
        {
            NodeElement parseNode(HtmlNode node)
            {
                NodeElement element = new NodeElement();
                element.Children = new List<NodeElement>();
                element.Attributes = new Dictionary<string, string>();
                element.Tag = node.Name;
                if (element.Tag == "script" ||
                    element.Tag == "style" ||
                    (element.Tag == "p" && node.Attributes.Where(x => x.Name == "class" && x.Value == "post_date").Count() != 0) ||
                    (element.Tag == "div" && node.Attributes.Where(x => x.Name == "class" && x.Value == "inner_post").Count() == 0))
                    return null;

                if (node.NodeType == HtmlNodeType.Text ||
                    node.Name == "#text")
                {
                    //element.Tag = "_text";
                    //element.Attributes["value"] = System.Web.HttpUtility.HtmlDecode(node.InnerText);
                    element.Children.Add(new NodeElement(System.Web.HttpUtility.HtmlDecode(node.InnerText)));
                    return element;
                }

                foreach (var childNode in node.ChildNodes)
                {
                    var childElement = parseNode(childNode);
                    if (childElement != null)
                        element.Children.Add(childElement);
                }

                foreach (var attribute in node.Attributes)
                {
                    element.Attributes[attribute.Name] = attribute.Value;
                }

                if (element.Children.Count == 0 &&
                    element.Attributes.Count == 0)
                    return null;

                return element;
            }
            var parsedNode = parseNode(this.Node);
            foreach (var node in parsedNode.Children.ToArray())
            {
                if (node.Attributes.Count() > 0)
                {
                    if (node.Attributes.First().Value.Replace(" ", "").Replace("\t", "").Replace("\n", "").Replace("\r", "") == "")
                        parsedNode.Children.Remove(node);
                }
            }
            
            return parsedNode.Children.ToArray();
        }

        public virtual bool Parse(HtmlNode node)
        {
            return false;
        }

        public bool Equals(BasePost other)
        {
            return this.Title == other.Title;
        }
    }
}
