using CommunityToolkit.Mvvm.ComponentModel;
using RA.DTO;
using RA.Logic.AudioPlayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.Playout.ViewModels.Components.Models
{
    public class TrackListingPlayerItem : ObservableObject, IPlayerItem
    {
        private readonly TrackListingDTO trackListingDTO;
        public string FilePath => trackListingDTO.FilePath;

        public string ImagePath
        {
            get
            {
                if (!string.IsNullOrEmpty(trackListingDTO.ImageName))
                {
                    //TODO
                    return $"C:\\Users\\Andrei\\Desktop\\images\\{trackListingDTO.ImageName}";
                }
                else
                {
                    return "pack://application:,,,/RA.UI.Core;component/Resources/Images/track_default_image.png";
                }

            }
        }

        public TimeSpan Duration => TimeSpan.FromSeconds(trackListingDTO.Duration);

        private DateTime eta;
        public DateTime ETA { get => eta; set => SetProperty(ref eta, value); }

        public string? Artists => trackListingDTO?.Artists;

        public string Title => trackListingDTO.Title;

        public string? TrackType => trackListingDTO.Type;

        public TrackListingPlayerItem(TrackListingDTO trackListingDTO)
        {
            this.trackListingDTO = trackListingDTO;
        }
    }
}
