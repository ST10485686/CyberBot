using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace CyberBot.Classes
{
    internal class ResponseManager
    {
        private static readonly Random _random = new Random();

        internal string GetResponse(string userInput, string userName)
        {
            // Simple response logic for the internal class
            if (string.IsNullOrWhiteSpace(userInput))
                return "I didn't catch that. Could you please say something?";

            string lowerInput = userInput.ToLower();

            // Basic keyword matching
            if (lowerInput.Contains("hello") || lowerInput.Contains("hi"))
                return $"Hello, {userName}! How can I help you with cybersecurity today?";

            if (lowerInput.Contains("password"))
                return "Password safety tips: Use unique, complex passwords with at least 12 characters, mix of uppercase, lowercase, numbers, and symbols.";

            if (lowerInput.Contains("phishing"))
                return "Phishing warning: Never click suspicious links in emails. Always verify the sender's email address.";

            if (lowerInput.Contains("browsing"))
                return "Safe browsing tips: Look for HTTPS in URLs, avoid public Wi-Fi for sensitive transactions, keep your browser updated.";

            if (lowerInput.Contains("help"))
                return "You can ask me about:\n• Password safety\n• Phishing prevention\n• Safe browsing habits\n• Account protection\n\nType 'exit' to quit.";

            return "I didn't quite understand that. Could you rephrase? Try asking about passwords, phishing, or safe browsing, or type 'help' for options.";
        }
    }
}

namespace CybersecurityBot
{
    [SupportedOSPlatform("windows")]
    public class ResponseManager
    {
        private Dictionary<string, List<string>> responsePatterns = new Dictionary<string, List<string>>(); // Initialize to avoid CS8618
        private static readonly Random _random = new Random();

        public ResponseManager()
        {
            InitializeResponses();
        }

        private void InitializeResponses()
        {
            responsePatterns = new Dictionary<string, List<string>>();

            // Password-related responses
            responsePatterns["password"] = new List<string>
            {
                "Password safety tips: Use unique, complex passwords with at least 12 characters, mix of uppercase, lowercase, numbers, and symbols.",
                "Never reuse passwords across accounts! Consider using a password manager.",
                "Enable 2FA (Two-Factor Authentication) whenever possible for an extra layer of security.",
                "A strong password should be at least 16 characters and avoid common words or personal information."
            };

            // Phishing-related responses
            responsePatterns["phishing"] = new List<string>
            {
                "Phishing warning: Never click suspicious links in emails. Always verify the sender's email address.",
                "Be cautious of urgent requests for personal information. Legitimate companies won't ask for passwords via email.",
                "Check URLs carefully - scammers use domains that look similar to real ones (e.g., amaz0n.com instead of amazon.com).",
                "Hover over links before clicking to see the actual destination URL."
            };

            // Safe browsing responses
            responsePatterns["browsing"] = new List<string>
            {
                "Safe browsing tips: Look for HTTPS in URLs, avoid public Wi-Fi for sensitive transactions, keep your browser updated.",
                "Use ad-blockers and avoid downloading files from untrusted websites.",
                "Clear your browser cache and cookies regularly to protect your privacy.",
                "Consider using a VPN when connecting to public Wi-Fi networks."
            };

            // General conversation responses
            responsePatterns["greeting"] = new List<string>
            {
                "I'm doing great, thanks for asking! Ready to help with cybersecurity questions.",
                "I'm functioning well! What cybersecurity topic would you like to learn about?",
                "All systems operational! How can I assist you with online safety today?"
            };

            responsePatterns["purpose"] = new List<string>
            {
                "I'm here to educate you about cybersecurity best practices! I can help with passwords, phishing, and safe browsing.",
                "My purpose is to help you stay safe online! Ask me about account safety, phishing prevention, or secure browsing habits.",
                "I'm your personal cybersecurity guide! I provide tips and education on staying protected online."
            };

            responsePatterns["help"] = new List<string>
            {
                "You can ask me about:\n• Password safety\n• Phishing prevention\n• Safe browsing habits\n• Account protection\n\nType 'exit' to quit.",
                "Available topics:\n- Password security\n- Phishing scams\n- Safe browsing\n- Account protection\n\nJust ask me about any of these!"
            };
        }

        public string GetResponse(string userInput, string userName)
        {
            if (string.IsNullOrWhiteSpace(userInput))
                return "I didn't catch that. Could you please say something?";

            string lowerInput = userInput.ToLower();

            // Check for goodbye/exit (personalized with user name)
            if (lowerInput.Contains("goodbye") || lowerInput.Contains("bye") || lowerInput.Contains("exit"))
                return $"Goodbye, {userName}! Stay safe online and remember to practice good cybersecurity habits!";

            // Check for greeting patterns (personalized)
            if (lowerInput.Contains("how are you") || lowerInput.Contains("how are u"))
                return GetRandomResponse("greeting");

            if (lowerInput.Contains("my name is") || lowerInput.Contains("call me"))
            {
                // Extract the name from the input (simple version)
                var words = userInput.Split(' ');
                var lastWord = words[words.Length - 1];
                return $"Nice to meet you, {lastWord}! I'm your cybersecurity assistant. How can I help you today?";
            }

            // Check for purpose question
            if (lowerInput.Contains("purpose") || lowerInput.Contains("what can you do") ||
                lowerInput.Contains("what do you do") || lowerInput.Contains("your function"))
                return GetRandomResponse("purpose");

            // Check for help
            if (lowerInput.Contains("help") || lowerInput.Contains("what can i ask") ||
                lowerInput.Contains("options") || lowerInput.Contains("commands"))
                return GetRandomResponse("help");

            // Check for thank you messages
            if (lowerInput.Contains("thank") || lowerInput.Contains("thanks") || lowerInput.Contains("appreciate"))
                return $"You're welcome, {userName}! Feel free to ask if you have more cybersecurity questions.";

            // Check for cybersecurity topics
            if (lowerInput.Contains("password") || lowerInput.Contains("account safety") ||
                lowerInput.Contains("secure account") || lowerInput.Contains("login") ||
                lowerInput.Contains("credentials"))
                return GetRandomResponse("password");

            if (lowerInput.Contains("phishing") || lowerInput.Contains("scam") ||
                lowerInput.Contains("email scam") || lowerInput.Contains("fraud") ||
                lowerInput.Contains("suspicious email"))
                return GetRandomResponse("phishing");

            if (lowerInput.Contains("browsing") || lowerInput.Contains("internet safety") ||
                lowerInput.Contains("web safety") || lowerInput.Contains("online safety") ||
                lowerInput.Contains("surfing"))
                return GetRandomResponse("browsing");

            // Check for general cybersecurity questions
            if (lowerInput.Contains("cyber") || lowerInput.Contains("security") ||
                lowerInput.Contains("safe") || lowerInput.Contains("protect"))
            {
                return $"That's a great question, {userName}! Cybersecurity is all about protecting your digital life. Could you be more specific? Try asking about passwords, phishing, or safe browsing.";
            }

            // Default response for unrecognized input (personalized)
            string[] defaultResponses = {
                $"I didn't quite understand that, {userName}. Could you rephrase? Try asking about passwords, phishing, or safe browsing, or type 'help' for options.",
                $"Hmm, I'm not sure about that, {userName}. Would you like to ask about password safety, phishing prevention, or secure browsing instead?",
                $"I'm still learning, {userName}! Could you ask me about specific cybersecurity topics like passwords, phishing, or safe browsing?"
            };

            return defaultResponses[_random.Next(defaultResponses.Length)];
        }

        private string GetRandomResponse(string category)
        {
            if (responsePatterns.ContainsKey(category) && responsePatterns[category].Count > 0)
            {
                var responses = responsePatterns[category];
                return responses[_random.Next(responses.Count)];
            }
            return "I'm not sure about that. Can you ask something else?";
        }
    }
}

namespace CyberBot.UI
{
    internal class UIManager
    {
        internal static void DisplayDivider()
        {
            Console.WriteLine(new string('-', 50));
        }

        internal void DisplayHeader()
        {
            Console.WriteLine("Welcome to Cybersecurity Bot!");
            Console.WriteLine("Ask me anything about online safety.");
            DisplayDivider();
        }

        internal string GetUserInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine() ?? string.Empty;
        }

        internal string GetUserName()
        {
            Console.Write("Enter your name: ");
            return Console.ReadLine() ?? string.Empty;
        }

        internal void TypeText(string message, ConsoleColor color, int delay = 40)
        {
            Console.ForegroundColor = color;
            foreach (char c in message)
            {
                Console.Write(c);
                System.Threading.Thread.Sleep(delay);
            }
            Console.ResetColor();
            Console.WriteLine();
        }

        internal void TypeText(string message, ConsoleColor color)
        {
            TypeText(message, color, 40);
        }
    }
}