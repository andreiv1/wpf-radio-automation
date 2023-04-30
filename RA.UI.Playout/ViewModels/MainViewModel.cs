using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using RA.Database;
using RA.DTO;
using RA.Logic.AudioPlayer;
using RA.Logic.AudioPlayer.Interfaces;
using RA.UI.Core.ViewModels;
using RA.UI.Playout.ViewModels.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RA.UI.Playout.ViewModels
{

    //TODO: Refactor
    public class AddedPlayerItem : IPlayerItem, INotifyPropertyChanged
    {
        public string FilePath => trackDto.FilePath;
        public TimeSpan Duration => TimeSpan.FromSeconds(trackDto.Duration);

        private DateTime eta;

        private TrackListDto trackDto;

        public event PropertyChangedEventHandler? PropertyChanged;

        public DateTime ETA
        {
            get => eta;
            set
            {
                eta = value;
                OnPropertyChanged(nameof(ETA));
            }
        }

        public AddedPlayerItem(int trackId)
        {
            using(var db = new AppDbContext())
            {
                trackDto = db.GetTrackById(trackId)
                    .Include(t => t.Categories)
                    .Select(t => TrackListDto.FromEntity(t))
                    .First();
            }
        }

        public string? Artists => trackDto.Artists;

        public string Title => trackDto.Title;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
    public partial class MainViewModel : ViewModelBase
    {
        public NowPlayingViewModel NowPlayingVm { get; private set; }
        public PlaylistViewModel PlaylistVm { get; private set; }
        public MediaItemsViewModel MediaItemsVm { get; private set; }

        public MainViewModel(NowPlayingViewModel nowPlayingVm, PlaylistViewModel playlistVm, 
            MediaItemsViewModel mediaItemsVm)
        {
            NowPlayingVm = nowPlayingVm;
            PlaylistVm = playlistVm;
            MediaItemsVm = mediaItemsVm;
            mediaItemsVm.MainVm = this;
            playlistVm.MainVm = this;
        }

        [RelayCommand]
        private void AddItemToPlaylistBottom()
        {
            if(MediaItemsVm.SelectedTrack is not null)
            {
                var track = MediaItemsVm.SelectedTrack;
                PlaylistVm.PlaybackAddItem(new AddedPlayerItem(track.Id));
            }
        }
    }
}
