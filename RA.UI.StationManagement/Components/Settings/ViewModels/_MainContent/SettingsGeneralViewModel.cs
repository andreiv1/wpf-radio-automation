using CommunityToolkit.Mvvm.ComponentModel;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.Settings.ViewModels.MainContent
{
    public partial class SettingsGeneralViewModel : ViewModelBase
    {
        private readonly ConfigurationStore configurationStore;

        [ObservableProperty]
        private string audioPath;

        [ObservableProperty]
        private string imagePath;

        [ObservableProperty]
        private TimeSpan artistSeparation;

        [ObservableProperty]
        private TimeSpan titleSeparation;

        [ObservableProperty]
        private TimeSpan trackSeparation;

        public SettingsGeneralViewModel(ConfigurationStore configurationStore)
        {
            this.configurationStore = configurationStore;

            AudioPath = configurationStore.AudioPath;
            ImagePath = configurationStore.ImagePath;
            ArtistSeparation = TimeSpan.FromMinutes(configurationStore.DefaultArtistSeparation);
            TitleSeparation = TimeSpan.FromMinutes(configurationStore.DefaultTitleSeparation);
            TrackSeparation = TimeSpan.FromMinutes(configurationStore.DefaultTrackSeparation);

        }
    }
}
