using System;
using System.Windows;

namespace CyberBotGUI
{
    public class ChatMessage
    {
        public string UserMessage { get; set; }
        public string BotMessage { get; set; }
        public string UserTime { get; set; }
        public string BotTime { get; set; }
        public Visibility ShowUser { get; set; }
        public Visibility ShowBot { get; set; }

        // Constructor for User message
        public static ChatMessage CreateUserMessage(string message)
        {
            return new ChatMessage
            {
                UserMessage = message,
                UserTime = DateTime.Now.ToString("HH:mm"),
                ShowUser = Visibility.Visible,
                ShowBot = Visibility.Collapsed,
                BotMessage = "",
                BotTime = ""
            };
        }

        // Constructor for Bot message
        public static ChatMessage CreateBotMessage(string message)
        {
            return new ChatMessage
            {
                BotMessage = message,
                BotTime = DateTime.Now.ToString("HH:mm"),
                ShowBot = Visibility.Visible,
                ShowUser = Visibility.Collapsed,
                UserMessage = "",
                UserTime = ""
            };
        }
    }
}