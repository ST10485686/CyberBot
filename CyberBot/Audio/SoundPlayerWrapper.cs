using System.Media;
using System.Threading;
using System.Threading.Tasks;

namespace CyberBot.Audio
{
    // Simple implementation using System.Media.SoundPlayer.
    // PlaySync runs until completion, so run it off the calling thread and return a Task.
    public class SoundPlayerWrapper : IAudioPlayer
    {
        public Task PlayAsync(string path, CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            {
                using var player = new SoundPlayer(path);
                player.Load();
                if (cancellationToken.IsCancellationRequested) return;
                player.PlaySync(); // blocks this background thread until audio finishes
            }, cancellationToken);
        }
    }
}