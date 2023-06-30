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
        private static String defaultImage = @"pack://application:,,,/RA.UI.Core;component/Resources/Images/track_default_image.png";

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
        private String image = defaultImage;

        public PlaylistViewModel PlaylistVm { get; set; }

        partial void OnDurationNowChanged(TimeSpan oldValue, TimeSpan newValue)
        {
            ProgressLabels.Clear();
            int totalSeconds = (int)newValue.TotalSeconds;

            // Calculate the ProgressTickFreq based on the song duration
            if (totalSeconds <= 60)
            {
                ProgressTickFreq = 5;
            }
            else if (totalSeconds <= 180)
            {
                ProgressTickFreq = 15;
            }
            else
            {
                ProgressTickFreq = 30;
            }
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
            Image = defaultImage;
            ResetOnlyTimers();
        }

        public void ResetOnlyTimers()
        {
            IsItemLoaded = false;
            IsPaused = false;
            ElapsedNow = TimeSpan.Zero;
            RemainingNow = null;
            DurationNow = TimeSpan.Zero;
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

        public void SeekTo(double position)
        {
            ElapsedNow = TimeSpan.FromSeconds(position);
            RemainingNow = DurationNow - ElapsedNow;
            PlaylistVm.SeekCommand.Execute(new TimeSpan[] { RemainingNow.Value, ElapsedNow});
        }
    }
}
