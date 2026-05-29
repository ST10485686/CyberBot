using System;
using System.Collections.Generic;

namespace CyberBotGUI
{
    public class ChatService
    {
        private Random random = new Random();

        // Dictionary for keyword responses (Code Optimization requirement)
        private Dictionary<string, List<string>> responses;

        // Memory storage (Memory and Recall requirement)
        private string userName = "";
        private string favoriteTopic = "";
        private List<string> conversationHistory = new List<string>();

        public ChatService()
        {
            InitializeResponses();
        }

        // Properties for memory access
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public string FavoriteTopic
        {
            get { return favoriteTopic; }
            set { favoriteTopic = value; }
        }

        public List<string> ConversationHistory
        {
            get { return conversationHistory; }
        }

        // Initialize all responses using Dictionary and Lists (Code Optimization)
        private void InitializeResponses()
        {
            responses = new Dictionary<string, List<string>>
            {
                // Password responses (Keyword 1)
                ["password"] = new List<string>
                {
                    "Use strong passwords with 12+ characters, mixing uppercase, lowercase, numbers, and symbols.",
                    "Never reuse passwords across accounts. Consider using a password manager like Bitwarden.",
                    "Enable Two-Factor Authentication (2FA) whenever possible for extra security.",
                    "Avoid using personal information like birthdays or names in your passwords.",
                    "A good password should be at least 16 characters long and hard to guess."
                },

                // Scam responses (Keyword 2)
                ["scam"] = new List<string>
                {
                    "Never click suspicious links in emails or text messages.",
                    "Legitimate companies will never ask for your password via email.",
                    "Always verify the sender's email address carefully - scammers use fake addresses.",
                    "If something seems too good to be true, it probably is a scam.",
                    "Scammers often create urgency to make you act without thinking."
                },

                // Privacy responses (Keyword 3)
                ["privacy"] = new List<string>
                {
                    "Review your privacy settings on social media regularly.",
                    "Use a VPN when connecting to public Wi-Fi networks.",
                    "Clear your browser cookies and cache regularly to protect your privacy.",
                    "Check which apps have access to your location, camera, and microphone.",
                    "Limit the personal information you share online."
                },

                // Phishing responses
                ["phishing"] = new List<string>
                {
                    "Phishing attacks try to trick you into giving away personal information.",
                    "Hover over links before clicking to see the actual URL destination.",
                    "Urgent messages claiming 'Your account will be closed' are often phishing attempts.",
                    "Never enter personal information on websites reached through email links.",
                    "Check for spelling errors in emails - they are common in phishing attempts."
                },

                // Safe browsing responses
                ["browsing"] = new List<string>
                {
                    "Look for HTTPS in URLs when visiting websites.",
                    "Avoid public Wi-Fi for sensitive transactions like banking.",
                    "Keep your browser updated for the latest security patches.",
                    "Clear your browser cache and cookies regularly.",
                    "Use ad-blockers to avoid malicious advertisements."
                },

                // Help responses
                ["help"] = new List<string>
                {
                    "I can help with:\n- Password safety\n- Scam detection\n- Privacy protection\n- Phishing prevention\n- Safe browsing\n\nJust ask me about any of these topics.",
                    "Try asking me:\n- 'Tell me about passwords'\n- 'How to spot a scam'\n- 'Privacy tips'\n- 'What is phishing?'\n- 'Safe browsing advice'",
                    "Available topics: passwords, scams, privacy, phishing, safe browsing. Just type your question!"
                },

                // Greeting responses
                ["greeting"] = new List<string>
                {
                    "Hello! How can I help you with cybersecurity today?",
                    "Hi there! Ready to learn about online safety?",
                    "Greetings! Ask me anything about staying safe online."
                },

                // Thank you responses
                ["thanks"] = new List<string>
                {
                    "You are welcome! Stay safe online.",
                    "Happy to help! Any other questions?",
                    "My pleasure! Cybersecurity is important for everyone."
                }
            };
        }

        // Main method to get response (Dynamic Responses requirement)
        public string GetResponse(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput))
                return "I didn't catch that. Could you please say something?";

            string lowerInput = userInput.ToLower();

            // Store in conversation history (Memory requirement)
            conversationHistory.Add($"User: {userInput}");

            // Check for goodbye
            if (lowerInput.Contains("goodbye") || lowerInput.Contains("bye") ||
                lowerInput.Contains("exit") || lowerInput.Contains("quit"))
            {
                return $"Goodbye, {userName}. Stay safe online. Remember to practice good cybersecurity habits.";
            }

            // Check for thank you (Random response)
            if (lowerInput.Contains("thank") || lowerInput.Contains("thanks"))
            {
                return GetRandomResponse("thanks");
            }

            // Check for greeting (Random response)
            if (lowerInput.Contains("hello") || lowerInput.Contains("hi") || lowerInput.Contains("hey"))
            {
                string greeting = GetRandomResponse("greeting");
                return string.IsNullOrEmpty(userName) ? greeting : $"{greeting} How are you today, {userName}?";
            }

            // Check for how are you
            if (lowerInput.Contains("how are you"))
            {
                return "I am doing great! Ready to help you stay safe online. How can I assist you today?";
            }

            // Name change memory update
            if (lowerInput.Contains("my name is") || lowerInput.Contains("call me"))
            {
                var words = userInput.Split(' ');
                var potentialName = words[words.Length - 1];
                if (potentialName.Length > 1)
                {
                    string oldName = userName;
                    userName = potentialName;
                    conversationHistory.Add($"System: Name changed from {oldName} to {userName}");
                    return $"I have updated my memory. I will call you {userName} from now on.";
                }
            }

            // Memory recall - ask about favorite topic
            if (!string.IsNullOrEmpty(favoriteTopic) &&
                (lowerInput.Contains("remember") || lowerInput.Contains("recall") ||
                 lowerInput.Contains("what do i like") || lowerInput.Contains("my interest")))
            {
                return $"You mentioned you are interested in {favoriteTopic}. Would you like to learn more about that topic?";
            }

            // Check for keywords and return random response (Random Responses requirement)
            foreach (var keyword in responses.Keys)
            {
                if (lowerInput.Contains(keyword))
                {
                    string response = GetRandomResponse(keyword);

                    // Add memory recall if applicable
                    if (!string.IsNullOrEmpty(favoriteTopic) && lowerInput.Contains(favoriteTopic))
                    {
                        response += $"\n\n(As someone interested in {favoriteTopic}, you might find this helpful.)";
                    }

                    conversationHistory.Add($"Bot: {response}");
                    return response;
                }
            }

            // Default response for unknown input (Error Handling requirement)
            string[] defaultResponses = {
                $"I am not sure I understand, {userName}. Could you try rephrasing?",
                $"Try asking about passwords, scams, privacy, phishing, or safe browsing.",
                $"Could you ask about specific topics like passwords, scams, or phishing?",
                $"I'm still learning. Please ask me about cybersecurity topics like passwords, scams, or privacy."
            };

            string defaultResponse = defaultResponses[random.Next(defaultResponses.Length)];
            conversationHistory.Add($"Bot: {defaultResponse}");
            return defaultResponse;
        }

        // Random response selector (Random Responses requirement)
        private string GetRandomResponse(string category)
        {
            if (responses.ContainsKey(category) && responses[category].Count > 0)
            {
                var responseList = responses[category];
                return responseList[random.Next(responseList.Count)];
            }
            return "I'm not sure about that. Can you ask something else?";
        }

        // Store user's favorite topic (Memory requirement)
        public void StoreFavoriteTopic(string message)
        {
            string lowerMsg = message.ToLower();

            if (lowerMsg.Contains("password") && string.IsNullOrEmpty(favoriteTopic))
            {
                favoriteTopic = "passwords";
                conversationHistory.Add($"System: User interested in {favoriteTopic}");
            }
            else if (lowerMsg.Contains("privacy") && string.IsNullOrEmpty(favoriteTopic))
            {
                favoriteTopic = "privacy";
                conversationHistory.Add($"System: User interested in {favoriteTopic}");
            }
            else if (lowerMsg.Contains("scam") && string.IsNullOrEmpty(favoriteTopic))
            {
                favoriteTopic = "scams";
                conversationHistory.Add($"System: User interested in {favoriteTopic}");
            }
            else if (lowerMsg.Contains("phishing") && string.IsNullOrEmpty(favoriteTopic))
            {
                favoriteTopic = "phishing";
                conversationHistory.Add($"System: User interested in {favoriteTopic}");
            }
            else if (lowerMsg.Contains("browsing") && string.IsNullOrEmpty(favoriteTopic))
            {
                favoriteTopic = "safe browsing";
                conversationHistory.Add($"System: User interested in {favoriteTopic}");
            }
        }

        // Get memory summary
        public string GetMemorySummary()
        {
            string summary = "Memory Summary:\n";
            if (!string.IsNullOrEmpty(userName))
                summary += $"- Name: {userName}\n";
            if (!string.IsNullOrEmpty(favoriteTopic))
                summary += $"- Favorite topic: {favoriteTopic}\n";
            summary += $"- Total conversation messages: {conversationHistory.Count}";
            return summary;
        }

        // Clear memory
        public void ClearMemory()
        {
            userName = "";
            favoriteTopic = "";
            conversationHistory.Clear();
        }
    }
}