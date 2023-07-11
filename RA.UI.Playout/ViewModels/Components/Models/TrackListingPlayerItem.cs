using CommunityToolkit.Mvvm.ComponentModel;
using RA.DTO;
using RA.Logic.AudioPlayer.Interfaces;
using RA.UI.Playout.Stores;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.Playout.ViewModels.Components.Models
{
    public class TrackListingPlayerItem : ObservableObject, IPlayerItem
    {
        private readonly TrackListingDTO trackListingDTO;
        private readonly ConfigurationStore configurationStore;

        public string FilePath => trackListingDTO.FilePath;

        public string ImagePath
        {
            get
            {
                if (!string.IsNullOrEmpty(trackListingDTO.ImageName))
                {
                    return configurationStore.GetFullImagePath(trackListingDTO.ImageName);
                }
                else
                {
                    return ConfigurationStore.GetDefaultImagePath();
                }

            }
        }

        public TimeSpan Duration => TimeSpan.FromSeconds(trackListingDTO.Duration);

        private DateTime eta;
        public DateTime ETA { get => eta; set => SetProperty(ref eta, value); }

        public string? Artists => trackListingDTO?.Artists;

        public string Title => trackListingDTO!.Title;

        public string? TrackType => trackListingDTO.Type;

        public int TrackId => trackListingDTO.Id;

        public TrackListingPlayerItem(TrackListingDTO trackListingDTO, ConfigurationStore configurationStore)
        {
            this.trackListingDTO = trackListingDTO;
            this.configurationStore = configurationStore;
        }
    }
}
