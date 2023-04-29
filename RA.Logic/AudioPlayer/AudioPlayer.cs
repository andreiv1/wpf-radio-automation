using NAudio.Wave;
using RA.Logic.AudioPlayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Logic.AudioPlayer
{
    public class AudioPlayer : IAudioPlayer, IDisposable
    {
        public event EventHandler PlaybackStopped;
        public event EventHandler PlaybackPaused;

        private WaveOutEvent? waveOut;
        private AudioFileReader? audioFileReader;
        private AudioPlayerState state;
        public AudioPlayerState State => state;
        public AudioPlayer()
        {
            state = AudioPlayerState.Stopped;
        }

        public void Play(IPlayerItem item)
        {
            Stop();
            audioFileReader = new AudioFileReader(item.FilePath);
            waveOut = new WaveOutEvent();
            waveOut.Init(audioFileReader);
            waveOut.PlaybackStopped += OnPlaybackStopped;
            waveOut.Play();
            state = AudioPlayerState.Playing;
        }

        public void Play()
        {
            if (waveOut is not null && state == AudioPlayerState.Paused)
            {
                waveOut.Play();
                state = AudioPlayerState.Playing;
            }
        }

        public void Pause()
        {
            if(waveOut is not null && state == AudioPlayerState.Playing)
            {
                waveOut.Pause();
                state = AudioPlayerState.Paused;

                PlaybackPaused?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Stop()
        {
            if (waveOut is not null)
            {
                waveOut.Stop();
                state = AudioPlayerState.Stopped;
            }
        }

        public void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            if(waveOut is not null)
                waveOut.Dispose();
            waveOut = null;
            if(audioFileReader is not null)
                audioFileReader.Dispose();
            audioFileReader = null;
            state = AudioPlayerState.Stopped;

            PlaybackStopped?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            if (waveOut is not null)
            {
                waveOut.Stop();
                waveOut.Dispose();
                waveOut = null;
            }

            if (audioFileReader is not null)
            {
                audioFileReader.Dispose();
                audioFileReader = null;
            }
        }
    }
}
