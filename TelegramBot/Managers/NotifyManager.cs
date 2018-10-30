using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Other;
using TelegramBot.Parsers;
using Telegraph.Net;
using Telegraph.Net.Models;

namespace TelegramBot.Managers
{
    public class NotifyManager
    {
        public TelegramBotClient TelegramClient;
        public ITokenClient TelegraphClient;

        public NotifyManager()
        {
            TelegramClient = new TelegramBotClient(Settings.Current.TelegramToken);
            TelegraphClient = new TelegraphClient().GetTokenClient(Settings.Current.TelegraphToken);
        }

        public void Notify(BasePost post)
        {
            string title = post.FormatTitle();
            string text = post.FormatText();
            string content = $"{title}\n\n{text}";

            var message = TelegramClient.SendTextMessageAsync(Settings.Current.ChatId, content, Telegram.Bot.Types.Enums.ParseMode.Html, true).Result;
            
            try
            {
                var nodes = post.buildNode();
                var page = TelegraphClient.CreatePageAsync("CS:GO Blog Post", nodes, "Valve", post.Url).Result;

                InlineKeyboardButton keyboardButton = new InlineKeyboardButton();
                keyboardButton.Text = "Preview";
                keyboardButton.Url = page.Url;
                TelegramClient.EditMessageReplyMarkupAsync(Settings.Current.ChatId, message.MessageId, new InlineKeyboardMarkup(keyboardButton));
            }
            catch (Exception ex)
            {
                Logger.Warn($"Failed building preview!\n{ex.ToString()}");
            }
        }

        public void NotifyNode(NodeElement[] content)
        {

        }
    }
}
