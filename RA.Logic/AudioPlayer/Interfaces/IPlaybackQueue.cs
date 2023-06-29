using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Logic.AudioPlayer.Interfaces
{
    public enum PlaybackMode
    {
        Manual,
        Auto,
        Loop
    }
    public interface IPlaybackQueue
    {
        event EventHandler PlaybackStarted;
        event EventHandler PlaybackStopped;
        event EventHandler PlaybackPaused;
        event EventHandler PlaybackItemChange;

        public IPlayerItem? NowPlaying { get; }
        public PlaybackMode Mode { get; set; }
        public void Play();
        public void Pause();
        public void Resume();
        public void Stop();
        public void AddItem(IPlayerItem item);
        public void RemoveItem(IPlayerItem item);
        public void MoveItem(IPlayerItem item, int index);
        public IPlayerItem? GetNextItem();
        public int GetQueueLength();
        public void ClearQueue();
        public void UpdateETAs(TimeSpan? initOffset);
        void AddItem(IPlayerItem item, int position = 0);
    }
}
