using CommunityToolkit.Mvvm.ComponentModel;
using RA.Database.Models.Enums;
using RA.DTO;
using RA.Logic.AudioPlayer.Interfaces;
using RA.Logic.Tracks;
using RA.UI.Playout.Stores;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.Playout.ViewModels.Components.Models
{
    public class TrackPlaylistPlayerItem : ObservableObject, IPlayerItem
    {
        private readonly PlaylistItemDTO playlistItemTrackDTO;
        private readonly ConfigurationStore configurationStore;

        public string FilePath => playlistItemTrackDTO.Track?.FilePath ?? "";

        public TimeSpan Duration => TimeSpan.FromSeconds(playlistItemTrackDTO.Track?.Duration ?? 0);

        private DateTime eta;
        public DateTime ETA { get => eta; set => SetProperty(ref eta, value); }
        public string? Artists => playlistItemTrackDTO.Track?.Artists ?? "unknown";

        public string Title => playlistItemTrackDTO.Track?.Title ?? "unknown";

        public string TrackType => playlistItemTrackDTO.Track?.Type ?? "unknown";

        public string ImagePath
        {
            get
            {
                if (!string.IsNullOrEmpty(playlistItemTrackDTO.Track?.ImageName))
                {
                    return Path.Combine(configurationStore.ImagePath, playlistItemTrackDTO.Track.ImageName);
                }
                else
                {
                    return "pack://application:,,,/RA.UI.Core;component/Resources/Images/track_default_image.png";
                }
                
            }
        }

        public int TrackId => playlistItemTrackDTO.Track?.Id ?? 0;

        public TrackPlaylistPlayerItem(PlaylistItemDTO playlistItemTrackDTO, ConfigurationStore configurationStore)
        {
            this.playlistItemTrackDTO = playlistItemTrackDTO;
            this.configurationStore = configurationStore;
        }
    }
}
