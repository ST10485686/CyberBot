using System;
using System.IO;
using System.Media;
using System.Windows.Threading;

namespace CyberBotGUI
{
    public class AudioService
    {
        private SoundPlayer soundPlayer;

        public AudioService()
        {
            soundPlayer = null;
        }

        // Play greeting audio when application starts
        public void PlayGreeting()
        {
            try
            {
                string audioPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Audio", "greeting.wav");

                // Also try alternative paths
                if (!File.Exists(audioPath))
                {
                    audioPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "greeting.wav");
                }

                if (File.Exists(audioPath))
                {
                    soundPlayer = new SoundPlayer(audioPath);
                    soundPlayer.Play(); // Non-blocking play
                }
                else
                {
                    Console.WriteLine("Audio file not found: " + audioPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not play audio: " + ex.Message);
            }
        }

        // Stop audio if needed
        public void StopAudio()
        {
            soundPlayer?.Stop();
        }

        // Play audio in a separate thread (non-blocking)
        public void PlayAudioAsync(string fileName)
        {
            try
            {
                string audioPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Audio", fileName);

                if (!File.Exists(audioPath))
                {
                    audioPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                }

                if (File.Exists(audioPath))
                {
                    System.Threading.Thread audioThread = new System.Threading.Thread(() =>
                    {
                        try
                        {
                            using (SoundPlayer player = new SoundPlayer(audioPath))
                            {
                                player.PlaySync(); // Plays and waits
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Audio playback error: " + ex.Message);
                        }
                    });
                    audioThread.IsBackground = true;
                    audioThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not play audio: " + ex.Message);
            }
        }
    }
}