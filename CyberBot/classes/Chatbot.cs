using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CyberBot.classes;

namespace CyberBot.Classes
{
    [SupportedOSPlatform("windows")]
    public class Chatbot
    {
        private string userName = string.Empty;
        private ResponseManager responseManager;
        private UIManager uiManager;

        public Chatbot()
        {
            responseManager = new ResponseManager();
            uiManager = new UIManager();
        }

        public void Run()
        {
            // 1. Play voice greeting (NOW NON-BLOCKING)
            PlayVoiceGreeting();

            // 2. Show ASCII art (runs immediately while audio plays)
            uiManager.DisplayHeader();

            // 3. Get user name (runs immediately while audio plays)
            userName = uiManager.GetUserName();

            // 4. Personal welcome
            uiManager.TypeText($"\nHello, {userName}! ", ConsoleColor.Green);
            uiManager.TypeText("I'm your Cybersecurity Awareness Bot.\n", ConsoleColor.Cyan);

            // 5. Start conversation
            StartConversation();
        }

        private void PlayVoiceGreeting()
        {
            // Start audio in background without blocking
            System.Threading.Thread audioThread = new System.Threading.Thread(() =>
            {
                try
                {
                    AudioPlayer.PlayGreeting();
                }
                catch (Exception ex)
                {
                    
                }
            });

            audioThread.IsBackground = true;  // Thread will close when program exits
            audioThread.Start();               // Start the audio in background

            // Small pause to let audio start (optional, makes it smoother)
            System.Threading.Thread.Sleep(100);
        }

        private void StartConversation()
        {
            bool running = true;

            while (running)
            {
                UIManager.DisplayDivider();
                string userInput = uiManager.GetUserInput($"{userName}: ");

                // Check for exit command
                if (userInput.ToLower() == "exit" || userInput.ToLower() == "quit")
                {
                    uiManager.TypeText($"Goodbye, {userName}! Stay safe online! \n", ConsoleColor.Yellow);
                    running = false;
                    continue;
                }

                // Get response from response manager
                string botResponse = responseManager.GetResponse(userInput, userName);

                // Display with typing effect
                uiManager.TypeText($"Bot: ", ConsoleColor.Cyan);
                uiManager.TypeText($"{botResponse}\n", ConsoleColor.White, 30);
            }
        }
    }
}