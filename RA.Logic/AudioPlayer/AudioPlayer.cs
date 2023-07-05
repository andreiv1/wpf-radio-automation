using NAudio.Wave;
using RA.Logic.AudioPlayer.Interfaces;
using System.ComponentModel;

namespace RA.Logic.AudioPlayer
{
    public class AudioPlayer : IAudioPlayer
    {
        public event EventHandler? PlaybackStopped;
        public event EventHandler? PlaybackPaused;
        public event PropertyChangedEventHandler? PropertyChanged;

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
            if (waveOut != null && state == AudioPlayerState.Paused)
            {
                waveOut.Play();
                state = AudioPlayerState.Playing;
            }
        }

        public void Pause()
        {
            if (waveOut != null && state == AudioPlayerState.Playing)
            {
                waveOut.Pause();
                state = AudioPlayerState.Paused;

                PlaybackPaused?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Stop()
        {
            if (waveOut != null)
            {
                waveOut.Stop();
                state = AudioPlayerState.Stopped;
            }
        }

        public void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            if (waveOut is not null)
                waveOut.Dispose();
            waveOut = null;
            if (audioFileReader is not null)
                audioFileReader.Dispose();
            audioFileReader = null;
            state = AudioPlayerState.Stopped;

            PlaybackStopped?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            if (waveOut != null)
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

        public void Resume()
        {
            if (waveOut != null && state == AudioPlayerState.Paused)
            {
                waveOut.Play();
                state = AudioPlayerState.Playing;
            }
        }

        public void Seek(TimeSpan position)
        {
            if (audioFileReader != null && waveOut != null)
            {
                //audioFileReader.Position = (long)(position.TotalSeconds * audioFileReader.WaveFormat.SampleRate * audioFileReader.WaveFormat.Channels * audioFileReader.WaveFormat.BitsPerSample / 8);
                //waveOut.Play();
                var pos = (long)(position.TotalSeconds * audioFileReader.WaveFormat.SampleRate * audioFileReader.WaveFormat.Channels * audioFileReader.WaveFormat.BitsPerSample / 8);
                audioFileReader.Seek(pos, SeekOrigin.Begin);
            }
        }
    }
}
