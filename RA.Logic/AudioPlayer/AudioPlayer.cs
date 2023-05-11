using NAudio.Wave;
using RA.Logic.AudioPlayer.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSoundVisualizationLib;

namespace RA.Logic.AudioPlayer
{
    public class AudioPlayer : IAudioPlayer, IDisposable, IWaveformPlayer, INotifyPropertyChanged
    {
        public event EventHandler PlaybackStopped;
        public event EventHandler PlaybackPaused;
        public event PropertyChangedEventHandler? PropertyChanged;

        private WaveOutEvent? waveOut;
        private AudioFileReader? audioFileReader;
        private AudioPlayerState state;
        public AudioPlayerState State => state;

        #region Constructor
        public AudioPlayer()
        {
            state = AudioPlayerState.Stopped;
        }

        #endregion

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
        #region IWaveformPlayer
        public double ChannelPosition { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public double ChannelLength => 0;

        private float[] waveformData;
        public float[] WaveformData
        {
            get { return waveformData; }
            protected set
            {
                float[] oldValue = waveformData;
                waveformData = value;
                if (oldValue != waveformData)
                    NotifyPropertyChanged("WaveformData");
            }
        }


        public TimeSpan SelectionBegin { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public TimeSpan SelectionEnd { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool IsPlaying => throw new NotImplementedException();

        #endregion

        #region Waveform Generation
        private class WaveformGenerationParams
        {
            public WaveformGenerationParams(int points, string path)
            {
                Points = points;
                Path = path;
            }

            public int Points { get; protected set; }
            public string Path { get; protected set; }
        }

        //private void GenerateWaveformData(string path)
        //{
        //    if (waveformGenerateWorker.IsBusy)
        //    {
        //        pendingWaveformPath = path;
        //        waveformGenerateWorker.CancelAsync();
        //        return;
        //    }

        //    if (!waveformGenerateWorker.IsBusy && waveformCompressedPointCount != 0)
        //        waveformGenerateWorker.RunWorkerAsync(new WaveformGenerationParams(waveformCompressedPointCount, path));
        //}

        //private void waveformGenerateWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    if (e.Cancelled)
        //    {
        //        if (!waveformGenerateWorker.IsBusy && waveformCompressedPointCount != 0)
        //            waveformGenerateWorker.RunWorkerAsync(new WaveformGenerationParams(waveformCompressedPointCount, pendingWaveformPath));
        //    }
        //}

        //private void waveformGenerateWorker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    WaveformGenerationParams waveformParams = e.Argument as WaveformGenerationParams;
        //    Mp3FileReader waveformMp3Stream = new Mp3FileReader(waveformParams.Path);
        //    WaveChannel32 waveformInputStream = new WaveChannel32(waveformMp3Stream);
        //    waveformInputStream.Sample += waveStream_Sample;

        //    int frameLength = fftDataSize;
        //    int frameCount = (int)((double)waveformInputStream.Length / (double)frameLength);
        //    int waveformLength = frameCount * 2;
        //    byte[] readBuffer = new byte[frameLength];
        //    waveformAggregator = new SampleAggregator(frameLength);

        //    float maxLeftPointLevel = float.MinValue;
        //    float maxRightPointLevel = float.MinValue;
        //    int currentPointIndex = 0;
        //    float[] waveformCompressedPoints = new float[waveformParams.Points];
        //    List<float> waveformData = new List<float>();
        //    List<int> waveMaxPointIndexes = new List<int>();

        //    for (int i = 1; i <= waveformParams.Points; i++)
        //    {
        //        waveMaxPointIndexes.Add((int)Math.Round(waveformLength * ((double)i / (double)waveformParams.Points), 0));
        //    }
        //    int readCount = 0;
        //    while (currentPointIndex * 2 < waveformParams.Points)
        //    {
        //        waveformInputStream.Read(readBuffer, 0, readBuffer.Length);

        //        waveformData.Add(waveformAggregator.LeftMaxVolume);
        //        waveformData.Add(waveformAggregator.RightMaxVolume);

        //        if (waveformAggregator.LeftMaxVolume > maxLeftPointLevel)
        //            maxLeftPointLevel = waveformAggregator.LeftMaxVolume;
        //        if (waveformAggregator.RightMaxVolume > maxRightPointLevel)
        //            maxRightPointLevel = waveformAggregator.RightMaxVolume;

        //        if (readCount > waveMaxPointIndexes[currentPointIndex])
        //        {
        //            waveformCompressedPoints[(currentPointIndex * 2)] = maxLeftPointLevel;
        //            waveformCompressedPoints[(currentPointIndex * 2) + 1] = maxRightPointLevel;
        //            maxLeftPointLevel = float.MinValue;
        //            maxRightPointLevel = float.MinValue;
        //            currentPointIndex++;
        //        }
        //        if (readCount % 3000 == 0)
        //        {
        //            float[] clonedData = (float[])waveformCompressedPoints.Clone();
        //            App.Current.Dispatcher.Invoke(new Action(() =>
        //            {
        //                WaveformData = clonedData;
        //            }));
        //        }

        //        if (waveformGenerateWorker.CancellationPending)
        //        {
        //            e.Cancel = true;
        //            break;
        //        }
        //        readCount++;
        //    }

        //    float[] finalClonedData = (float[])waveformCompressedPoints.Clone();
        //    App.Current.Dispatcher.Invoke(new Action(() =>
        //    {
        //        fullLevelData = waveformData.ToArray();
        //        WaveformData = finalClonedData;
        //    }));
        //    waveformInputStream.Close();
        //    waveformInputStream.Dispose();
        //    waveformInputStream = null;
        //    waveformMp3Stream.Close();
        //    waveformMp3Stream.Dispose();
        //    waveformMp3Stream = null;
        //}
        #endregion


        #region INotifyPropertyChanged
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion
    }
}
