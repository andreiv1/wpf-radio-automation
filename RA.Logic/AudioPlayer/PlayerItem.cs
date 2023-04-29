using RA.Dto;
using RA.Logic.AudioPlayer.Interfaces;
using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Logic.AudioPlayer
{
    public class PlayerItem : IPlayerItem, INotifyPropertyChanged
    {
        private readonly PlaylistItemTrackDto track;

        public event PropertyChangedEventHandler? PropertyChanged;
        public string FilePath => track.FilePath;
        public TimeSpan Duration => TimeSpan.FromSeconds(track.Duration);

        private DateTime eta;
        public DateTime ETA { get => eta;
            set
            {
                eta = value;
                OnPropertyChanged(nameof(ETA));
            }
        }

        public string? Artists => track.Artists;

        public string Title => track.Title;

        public PlayerItem()
        {

        }
        public PlayerItem(PlaylistItemDto playlistItemDto)
        {
            this.track = playlistItemDto.TrackDto;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
