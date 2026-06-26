using System;
using System.IO;
using System.Media;

namespace CyberBotGUI
{
    public class AudioService
    {
        public static void PlayGreetingAsync()
        {
            try
            {
                System.Threading.Thread audioThread = new System.Threading.Thread(() =>
                {
                    try
                    {
                        string[] possiblePaths = {
                            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "greeting.wav"),
                            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Audio", "greeting.wav")
                        };

                        string audioPath = null;
                        foreach (var path in possiblePaths)
                        {
                            if (File.Exists(path))
                            {
                                audioPath = path;
                                break;
                            }
                        }

                        if (audioPath != null)
                        {
                            using (SoundPlayer player = new SoundPlayer(audioPath))
                            {
                                player.PlaySync();
                            }
                        }
                    }
                    catch { }
                });
                audioThread.IsBackground = true;
                audioThread.Start();
            }
            catch { }
        }
    }
}