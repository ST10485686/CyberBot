using System;
using System.IO;
using System.Runtime.Versioning;

namespace CyberBot.classes
{
    public class AudioPlayer
    {
        [SupportedOSPlatform("windows")]
        public static void PlayGreeting()
        {
            try
            {
                string audioPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "greeting.wav");

                if (File.Exists(audioPath))
                {
                    // Use Windows Media Player as fallback
                    Type? playerType = Type.GetType("System.Media.SoundPlayer, System.Windows.Extensions");
                    if (playerType != null)
                    {
                        // CS8600 fix: Use 'as' and null-check before using player
                        dynamic? player = Activator.CreateInstance(playerType, new object[] { audioPath });
                        if (player != null)
                        {
                            player.Play();
                            Console.WriteLine("Playing welcome sound...");
                        }
                        else
                        {
                            Console.Beep(800, 300);
                            Console.WriteLine("[Beep] - Welcome!");
                        }
                    }
                    else
                    {
                        // If no audio, just beep
                        Console.Beep(800, 300);
                        Console.WriteLine("[Beep] - Welcome!");
                    }
                }
                else
                {
                    Console.WriteLine("Welcome!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Note: Audio not available ({ex.Message})");
            }
        }
    }
}