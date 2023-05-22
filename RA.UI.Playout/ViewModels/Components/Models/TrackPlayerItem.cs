using CommunityToolkit.Mvvm.ComponentModel;
using RA.Database.Models.Enums;
using RA.DTO;
using RA.Logic.AudioPlayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.Playout.ViewModels.Components.Models
{
    public class TrackPlayerItem : ObservableObject, IPlayerItem
    {
        private readonly PlaylistItemTrackDTO playlistItemTrackDTO;

        public string FilePath => playlistItemTrackDTO.Track.FilePath;

        public TimeSpan Duration => TimeSpan.FromSeconds(playlistItemTrackDTO.Track.Duration);

        private DateTime eta;
        public DateTime ETA { get => eta; set => SetProperty(ref eta, value); }
        public string? Artists => playlistItemTrackDTO.Track.Artists;

        public string Title => playlistItemTrackDTO.Track.Title;

        public string TrackType => playlistItemTrackDTO.Track.Type;

        public string ImagePath
        {
            get
            {
                if (!string.IsNullOrEmpty(playlistItemTrackDTO.Track.ImageName))
                {
                    //TODO
                    return $"C:\\Users\\Andrei\\Desktop\\images\\{playlistItemTrackDTO.Track.ImageName}";
                }
                else
                {
                    return "pack://application:,,,/RA.UI.Core;component/Resources/Images/track_default_image.png";
                }
                
            }
        }

        public TrackPlayerItem(PlaylistItemTrackDTO playlistItemTrackDTO)
        {
            this.playlistItemTrackDTO = playlistItemTrackDTO;
        }
    }
}
