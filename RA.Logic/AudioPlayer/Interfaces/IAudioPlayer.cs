using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Logic.AudioPlayer.Interfaces
{
    public enum AudioPlayerState
    {
        Stopped,
        Playing,
        Paused,
    }
    public interface IAudioPlayer
    {
        event EventHandler PlaybackStopped;
        event EventHandler PlaybackPaused;
        public AudioPlayerState State { get; }
        public void Play(IPlayerItem item);
        public void Play();
        public void Stop();
        public void Pause();
        public void Resume();
        public void Seek(TimeSpan position);
    }
}
