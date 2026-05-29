using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace CyberBotGUI
{
    public partial class MainWindow : Window
    {
        private ChatService chatService;
        private Random random = new Random();
        private string currentSentiment = "neutral";
        private AudioService audioService;  // Audio service

        // Sentiment responses
        private Dictionary<string, List<string>> sentimentResponses;

        public MainWindow()
        {
            InitializeComponent();

            // Initialize audio service
            audioService = new AudioService();

            // Play voice greeting when window loads
            Loaded += MainWindow_Loaded;

            InitializeSentimentResponses();
            chatService = new ChatService();
            AddWelcomeMessage();
            UpdateMemoryStatus();
        }

        // Play audio when window loads
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Play the voice greeting
            audioService.PlayGreeting();
        }

        private void InitializeSentimentResponses()
        {
            sentimentResponses = new Dictionary<string, List<string>>
            {
                ["worried"] = new List<string>
                {
                    "I understand your concern. It is completely normal to feel worried about online security. Let me share some reassuring tips.",
                    "Your safety matters. Don't worry - I am here to help you stay protected. Here is what you can do.",
                    "Feeling worried is understandable. The good news is there are simple steps you can take to protect yourself."
                },
                ["curious"] = new List<string>
                {
                    "Great question. I am glad you are curious about cybersecurity. Let me explain.",
                    "That is an excellent topic to explore. Here is what you should know.",
                    "I am glad you asked. Curiosity is the first step to better online safety."
                },
                ["frustrated"] = new List<string>
                {
                    "I hear your frustration. Cybersecurity can feel overwhelming. Let me simplify this for you.",
                    "Take a deep breath. I will break this down into simple, actionable steps.",
                    "I understand it can be frustrating. Let me help you understand this better."
                },
                ["neutral"] = new List<string>
                {
                    "Here is some helpful information for you.",
                    "Let me share some cybersecurity tips.",
                    "Thanks for asking. Here is what you should know."
                }
            };
        }

        private void AddWelcomeMessage()
        {
            ChatDisplay.Items.Add(ChatMessage.BotMessage(
                "Hello. Welcome to the Cybersecurity Awareness Bot.\n\n" +
                "I can help you learn about:\n" +
                "  - Password safety\n" +
                "  - Scam detection\n" +
                "  - Privacy protection\n" +
                "  - Phishing prevention\n" +
                "  - Safe browsing\n\n" +
                "I can also detect your sentiment and remember our conversation.\n\n" +
                "What is your name?"
            ));
        }

        private void UpdateMemoryStatus()
        {
            string status = "";
            if (!string.IsNullOrEmpty(chatService.UserName))
            {
                status += $"Name: {chatService.UserName}";
            }
            if (!string.IsNullOrEmpty(chatService.FavoriteTopic))
            {
                if (!string.IsNullOrEmpty(status)) status += " | ";
                status += $"Interest: {chatService.FavoriteTopic}";
            }
            if (string.IsNullOrEmpty(status))
            {
                status = "No user data stored yet";
            }
            else
            {
                status = $"Stored: {status} | Messages: {chatService.ConversationHistory.Count}";
            }
            MemoryStatus.Text = status;
        }

        // Suggestion button handler
        private void SuggestionButton_Click(object sender, RoutedEventArgs e)
        {
            string suggestion = "";

            if (sender == BtnPassword)
                suggestion = "Tell me about password safety";
            else if (sender == BtnScam)
                suggestion = "How to spot a scam";
            else if (sender == BtnPrivacy)
                suggestion = "Give me privacy tips";
            else if (sender == BtnPhishing)
                suggestion = "What is phishing";
            else if (sender == BtnBrowsing)
                suggestion = "Safe browsing tips";
            else if (sender == BtnHelp)
                suggestion = "help";

            UserInput.Text = suggestion;
            ProcessUserInput();
        }

        // Clear chat button handler
        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ChatDisplay.Items.Clear();
            chatService.ClearMemory();
            currentSentiment = "neutral";
            SentimentIndicator.Text = "NEUTRAL";
            SentimentIndicator.Foreground = new SolidColorBrush(Color.FromRgb(136, 136, 136));
            AddWelcomeMessage();
            UpdateMemoryStatus();
        }

        // Show memory button handler
        private void BtnMemory_Click(object sender, RoutedEventArgs e)
        {
            string memorySummary = chatService.GetMemorySummary();
            ChatDisplay.Items.Add(ChatMessage.BotMessage(memorySummary));
            ChatDisplay.ScrollIntoView(ChatDisplay.Items[ChatDisplay.Items.Count - 1]);
        }

        // Exit button handler
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            string userName = chatService.UserName;
            if (!string.IsNullOrEmpty(userName))
            {
                MessageBox.Show($"Goodbye, {userName}. Stay safe online!",
                                "Cybersecurity Bot",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
            Application.Current.Shutdown();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessUserInput();
        }

        private void UserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ProcessUserInput();
            }
        }

        private void ProcessUserInput()
        {
            string userMessage = UserInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(userMessage))
                return;

            ChatDisplay.Items.Add(ChatMessage.UserMessage(userMessage));
            UserInput.Clear();

            if (string.IsNullOrEmpty(chatService.UserName))
            {
                chatService.UserName = userMessage;
                ChatDisplay.Items.Add(ChatMessage.BotMessage(
                    $"Nice to meet you, {chatService.UserName}.\n\n" +
                    "I will remember your name. What cybersecurity topic interests you today?\n\n" +
                    "You can ask about: passwords, scams, privacy, phishing, or safe browsing."
                ));
                UpdateMemoryStatus();
                UserInput.Focus();
                return;
            }

            if (userMessage.ToLower() == "exit" || userMessage.ToLower() == "quit")
            {
                ChatDisplay.Items.Add(ChatMessage.BotMessage(
                    $"Goodbye, {chatService.UserName}. Stay safe online."
                ));
                return;
            }

            DetectSentiment(userMessage);
            UpdateSentimentIndicator();
            chatService.StoreFavoriteTopic(userMessage);
            string response = chatService.GetResponse(userMessage);

            if (currentSentiment != "neutral")
            {
                string sentimentPrefix = GetSentimentPrefix();
                response = sentimentPrefix + "\n\n" + response;
            }

            ChatDisplay.Items.Add(ChatMessage.BotMessage(response));
            UpdateMemoryStatus();
            ChatDisplay.ScrollIntoView(ChatDisplay.Items[ChatDisplay.Items.Count - 1]);
        }

        private void DetectSentiment(string message)
        {
            string lowerMsg = message.ToLower();

            if (lowerMsg.Contains("worried") || lowerMsg.Contains("scared") ||
                lowerMsg.Contains("nervous") || lowerMsg.Contains("anxious") ||
                lowerMsg.Contains("concerned") || lowerMsg.Contains("afraid"))
            {
                currentSentiment = "worried";
            }
            else if (lowerMsg.Contains("curious") || lowerMsg.Contains("interested") ||
                     lowerMsg.Contains("wonder") || lowerMsg.Contains("tell me") ||
                     lowerMsg.Contains("explain") || lowerMsg.Contains("learn"))
            {
                currentSentiment = "curious";
            }
            else if (lowerMsg.Contains("frustrated") || lowerMsg.Contains("confused") ||
                     lowerMsg.Contains("annoyed") || lowerMsg.Contains("difficult") ||
                     lowerMsg.Contains("hard") || lowerMsg.Contains("complicated"))
            {
                currentSentiment = "frustrated";
            }
            else
            {
                currentSentiment = "neutral";
            }
        }

        private void UpdateSentimentIndicator()
        {
            SentimentIndicator.Text = currentSentiment.ToUpper();

            switch (currentSentiment)
            {
                case "worried":
                    SentimentIndicator.Foreground = new SolidColorBrush(Color.FromRgb(255, 165, 0));
                    break;
                case "curious":
                    SentimentIndicator.Foreground = new SolidColorBrush(Color.FromRgb(76, 175, 80));
                    break;
                case "frustrated":
                    SentimentIndicator.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                    break;
                default:
                    SentimentIndicator.Foreground = new SolidColorBrush(Color.FromRgb(136, 136, 136));
                    break;
            }
        }

        private string GetSentimentPrefix()
        {
            var responses = sentimentResponses[currentSentiment];
            return responses[random.Next(responses.Count)];
        }
    }
}