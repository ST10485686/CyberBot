using System;
using System.IO;
using System.Media;
using System.Runtime.Versioning;
using System.Threading;  // ADD THIS
using System.Threading.Tasks;  // ADD THIS
using CyberBot.Classes;

namespace CyberBot
{
    public class Program
    {
        [SupportedOSPlatform("windows")]
        public static void PlayGreeting()
        {
            try
            {
                string fullPath = Path.Combine(AppContext.BaseDirectory, "Audio", "greeting.wav");

                if (!File.Exists(fullPath))
                {
                    Console.WriteLine("Note: Sound file not found at: " + fullPath);
                    return;
                }

                using SoundPlayer player = new(fullPath);
                player.Load();
                player.Play();  // CHANGED to Play() - non-blocking
                Thread.Sleep(100); // Give it time to start
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public static int Main(string[] args)
        {
            try
            {
                if (OperatingSystem.IsWindows())
                {
                    // Play audio in background
                    Task.Run(() => PlayGreeting());
                }
                else
                {
                    Console.WriteLine("PlayGreeting is supported only on Windows.");
                }

                // Start chatbot IMMEDIATELY while audio plays
                Console.WriteLine("Starting Cybersecurity Bot...");
                Chatbot chatbot = new Chatbot();
                chatbot.Run();

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                return 1;
            }
        }
    }
}