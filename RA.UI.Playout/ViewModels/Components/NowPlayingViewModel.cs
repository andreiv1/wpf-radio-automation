using CommunityToolkit.Mvvm.ComponentModel;
using RA.UI.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.Playout.ViewModels.Controls
{
    public partial class NowPlayingViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string nowArtist;

        [ObservableProperty]
        private string nowTitle;

        [ObservableProperty]
        private TimeSpan? remainingNow = null;

        [ObservableProperty]
        private TimeSpan elapsedNow;

        [ObservableProperty]
        private TimeSpan durationNow = TimeSpan.FromMinutes(1);

        [ObservableProperty]
        private bool isItemLoaded;

        [ObservableProperty]
        private bool isPaused;

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
        public void UpdateNowPlaying(String artist, String title, TimeSpan duration)
        {
            IsItemLoaded = true;
            IsPaused = false;
            NowArtist = artist;
            NowTitle = title;
            ElapsedNow = TimeSpan.Zero;
            RemainingNow = duration;
            DurationNow = duration;
        }
    }
}
