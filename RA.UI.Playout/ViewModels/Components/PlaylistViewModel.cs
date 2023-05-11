using RA.DAL;
using RA.DTO;
using RA.Logic;
using RA.Logic.AudioPlayer.Interfaces;
using RA.UI.Core.ViewModels;
using RA.UI.Playout.ViewModels.Components.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.Playout.ViewModels.Components
{
    public partial class PlaylistViewModel : ViewModelBase
    {
        private readonly IPlaybackQueue playbackQueue;
        private readonly IPlaylistsService playlistsService;
        private IPlayerItem? playerItemNow;
        public MainViewModel MainVm { get; set; }

        public ObservableCollection<IPlayerItem> PlayerItems { get; } = new();

        #region Constructor
        public PlaylistViewModel(IPlaybackQueue playbackQueue, IPlaylistsService playlistsService)
        {
            this.playbackQueue = playbackQueue;
            this.playlistsService = playlistsService;

            LoadPlaylist(DateTime.Now,1);
        }

        #endregion

        #region Data fetching
        private void LoadPlaylist(DateTime date, int maxHours = 1)
        {
            var playlistItems = playlistsService.GetPlaylistItemsByDateTime(date, maxHours).ToList();
            foreach (var item in playlistItems)
            {
                if (item?.GetType() == typeof(PlaylistItemTrackDTO))
                {
                    var trackDto = (PlaylistItemTrackDTO)item;
                    PlaybackAddItem(new TrackPlayerItem(trackDto));
                }

            }
        }

        #endregion

        #region Playback queue logic
        public void PlaybackAddItem(IPlayerItem playerItem)
        {
            playbackQueue.AddItem(playerItem);
            PlayerItems.Add(playerItem);
            if (MainVm is not null)
            {
                playbackQueue.UpdateETAs(MainVm.NowPlayingVm?.RemainingNow ?? null);
            }
            else
            {
                playbackQueue.UpdateETAs(null);
            }
        }
        #endregion
    }
}
