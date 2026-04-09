using System.Threading;
using System.Threading.Tasks;

namespace CyberBot.Audio
{
    public interface IAudioPlayer
    {
        // Plays the audio and completes the returned Task when playback finishes or is cancelled.
        Task PlayAsync(string path, CancellationToken cancellationToken = default);
    }
}