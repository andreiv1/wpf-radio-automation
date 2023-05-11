using RA.Logic.AudioPlayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Logic.AudioPlayer
{
    public class PlaybackQueue : IPlaybackQueue
    {
        private readonly List<IPlayerItem> list;
        private readonly IAudioPlayer audioPlayer;
        private PlaybackMode mode = PlaybackMode.Manual;

        public event EventHandler? PlaybackStarted;
        public event EventHandler? PlaybackStopped;
        public event EventHandler? PlaybackPaused;
        public event EventHandler? PlaybackItemChange;

        private IPlayerItem? nowPlaying = null;
        public IPlayerItem? NowPlaying => nowPlaying;
        public PlaybackQueue(IAudioPlayer audioPlayer)
        {
            list = new List<IPlayerItem>();
            this.audioPlayer = audioPlayer;
            this.audioPlayer.PlaybackStopped += OnPlaybackStopped;
            this.audioPlayer.PlaybackPaused += OnPlaybackPaused;
            StartUpdateETAs();
        }

        private void OnPlaybackPaused(object? sender, EventArgs e)
        {
            PlaybackPaused?.Invoke(this, e);

            StartUpdateETAs();
        }

        private void OnPlaybackStopped(object? sender, EventArgs e)
        {
            switch (mode)
            {
                case PlaybackMode.Auto:
                    if (list.Count > 0)
                    {
                        Play();
                    } else
                    {
                        nowPlaying = null;
                    }
                    break;
                case PlaybackMode.Manual:
                    nowPlaying = null;
                    break;
                case PlaybackMode.Loop:
                    Loop();
                    break;
            }
            PlaybackStopped?.Invoke(this, e);

            StartUpdateETAs();
        }

        private bool isUpdatingETAs = false;
        private System.Timers.Timer? etaTimer = null;

        private void StartUpdateETAs()
        {
            if (!isUpdatingETAs && (nowPlaying == null || audioPlayer.State == AudioPlayerState.Paused))
            {
                // Start the timer to update the ETAs
                etaTimer = new System.Timers.Timer(1000);
                etaTimer.Elapsed += OnUpdateETAs;
                etaTimer.AutoReset = true;
                etaTimer.Start();
                isUpdatingETAs = true;
            }
        }

        private void StopUpdateETAs()
        {
            if (isUpdatingETAs)
            {
                etaTimer?.Stop();
                etaTimer.Dispose();
                isUpdatingETAs = false;
            }
        }

        private void OnUpdateETAs(object? sender, System.Timers.ElapsedEventArgs e)
        {
            UpdateETAs(null);
            if (nowPlaying != null)
            {
                StopUpdateETAs();
            }
        }

        public PlaybackMode Mode
        {
            get => mode;
            set
            {
                mode = value;
            }
        }

        public void Play()
        {
            if(list.Count > 0)
            {
                if(nowPlaying is not null)
                {
                    audioPlayer.Stop();
                }
                nowPlaying = list.ElementAt(0);
                list.RemoveAt(0);
                audioPlayer.Play(nowPlaying); 
                
                Task.Delay(500).ContinueWith(t =>
                {
                    if(nowPlaying != null) 
                    UpdateETAs(nowPlaying.Duration); // Update ETAs after playing a new item.
                });
                
                PlaybackStarted?.Invoke(this, EventArgs.Empty);
            }

        }

        private void Loop()
        {
            if(nowPlaying is not null)
            {
                audioPlayer.Play(nowPlaying);

                Task.Delay(500).ContinueWith(t =>
                {
                    if (nowPlaying != null)
                        UpdateETAs(nowPlaying.Duration); // Update ETAs after playing a new item.
                });
            }
        }

        public void Pause()
        {
            audioPlayer.Pause();
        }

        public void Resume()
        {
            audioPlayer.Play();
        }
        public void Stop()
        {
            audioPlayer.Stop();
        }

        public void UpdateETAs(TimeSpan? initOffset)
        {
            TimeSpan offset = TimeSpan.Zero;
            if (initOffset.HasValue)
            {
                offset += initOffset.Value;
            }
            DateTime now = DateTime.Now;

            if(list.Count > 0)
            {
                list[0].ETA = now + offset;
            }
            for (int i = 1; i < list.Count; i++)
            {
                var previousItem = list[i - 1];
                var currentItem = list[i];
                offset += previousItem.Duration;
                currentItem.ETA = now + offset;
            }
        }

        public void AddItem(IPlayerItem item)
        {
            list.Add(item);
            UpdateETAs(null); // Update ETAs after adding an item.
        }

        public void RemoveItem(IPlayerItem item)
        {
            list.Remove(item);
            UpdateETAs(null);
        }

        public void MoveItem(IPlayerItem item, int index)
        {
            if(index  < 0 || index >= list.Count) { return; }

            list.Remove(item);
            list.Insert(index, item);
            UpdateETAs(null);
        }
        public IPlayerItem? GetNextItem()
        {
            return list.Count > 0 ? list.ElementAt(0) : null;
        }

        public int GetQueueLength()
        {
            return list.Count;
        }

        public void ClearQueue()
        {
            list.Clear();
        }

        
    }
}
