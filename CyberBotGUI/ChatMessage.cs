using System.Windows.Media;

namespace CyberBotGUI
{
    public class ChatMessage
    {
        public string Message { get; set; }
        public Brush Background { get; set; }
        public Brush Foreground { get; set; }

        public static ChatMessage UserMessage(string text)
        {
            return new ChatMessage
            {
                Message = $"You: {text}",
                Background = new SolidColorBrush(Color.FromRgb(45, 45, 45)),
                Foreground = new SolidColorBrush(Colors.LightGray)
            };
        }

        public static ChatMessage BotMessage(string text)
        {
            return new ChatMessage
            {
                Message = $"Bot: {text}",
                Background = new SolidColorBrush(Color.FromRgb(30, 30, 30)),
                Foreground = new SolidColorBrush(Colors.White)
            };
        }
    }
}