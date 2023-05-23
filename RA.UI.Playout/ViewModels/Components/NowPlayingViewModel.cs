using CommunityToolkit.Mvvm.ComponentModel;
using RA.UI.Core.ViewModels;
using Syncfusion.Windows.Controls.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.Playout.ViewModels.Components
{
    public partial class NowPlayingViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string nowArtist = "-";

        [ObservableProperty]
        private string nowTitle = "-";

        [ObservableProperty]
        private TimeSpan? remainingNow = null;

        [ObservableProperty]
        private TimeSpan elapsedNow;

        [ObservableProperty]
        private TimeSpan durationNow = TimeSpan.FromMinutes(1);

        [ObservableProperty]
        private String image;

        partial void OnDurationNowChanged(TimeSpan oldValue, TimeSpan newValue)
        {
            ProgressLabels.Clear();
            int totalSeconds = (int)newValue.TotalSeconds;

            //TODO:
            //if (totalSeconds <= 30)
            //{
            //    ProgressTickFreq = 30;

            //}
            ProgressTickFreq = 30;
            for (int i = ProgressTickFreq; i < totalSeconds; i += ProgressTickFreq)
            {
                TimeSpan labelTime = TimeSpan.FromSeconds(i);
                string label = labelTime.ToString(@"mm\:ss");
                ProgressLabels.Add(new Items { label = label, value = i });
            }
        }

        [ObservableProperty]
        private bool isItemLoaded;

        [ObservableProperty]
        private bool isPaused;

        public ObservableCollection<Items> ProgressLabels { get; set; } = new();

        [ObservableProperty]
        private int progressTickFreq = 0;

        public NowPlayingViewModel()
        {
            
        }
        public void Reset()
        {
            NowArtist = String.Empty;
            NowTitle = String.Empty;
            ResetOnlyTimers();
        }

        public void ResetOnlyTimers()
        {
            IsItemLoaded = false;
            IsPaused = false;
            ElapsedNow = TimeSpan.Zero;
            RemainingNow = null;
            DurationNow = TimeSpan.FromMinutes(1);
        }
        public void UpdateNowPlaying(String artist, String title, TimeSpan duration, String image)
        {
            IsItemLoaded = true;
            IsPaused = false;
            NowArtist = artist;
            NowTitle = title;
            ElapsedNow = TimeSpan.Zero;
            RemainingNow = duration;
            DurationNow = duration;
            Image = image;
        }
    }
}
