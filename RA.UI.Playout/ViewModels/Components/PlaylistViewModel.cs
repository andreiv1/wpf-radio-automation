using RA.DAL;
using RA.DTO;
using RA.Logic;
using RA.Logic.AudioPlayer.Interfaces;
using RA.UI.Core.Services;
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
        private readonly IDispatcherService dispatcherService;
        private readonly IPlaybackQueue playbackQueue;
        private readonly IPlaylistsService playlistsService;
        private IPlayerItem? playerItemNow;
        public MainViewModel MainVm { get; set; }

        public ObservableCollection<IPlayerItem> PlayerItems { get; } = new();

        #region Constructor
        public PlaylistViewModel(IDispatcherService dispatcherService,
                                 IPlaybackQueue playbackQueue,
                                 IPlaylistsService playlistsService)
        {
            this.dispatcherService = dispatcherService;
            this.playbackQueue = playbackQueue;
            this.playlistsService = playlistsService;

            _ = LoadPlaylist(DateTime.Now,1);
        }

        #endregion

        #region Data fetching
        private async Task LoadPlaylist(DateTime date, int maxHours = 1)
        {
            await Task.Run(() => {
                var playlistItems = playlistsService.GetPlaylistItemsByDateTime(date, maxHours);
                foreach (var item in playlistItems)
                {
                    if (item?.GetType() == typeof(PlaylistItemTrackDTO))
                    {
                        var trackDto = (PlaylistItemTrackDTO)item;
                        PlaybackAddItem(new TrackPlayerItem(trackDto));
                    }

                }
            });
        }

        #endregion

        #region Playback queue logic
        public void PlaybackAddItem(IPlayerItem playerItem)
        {
            playbackQueue.AddItem(playerItem);
            dispatcherService.InvokeOnUIThread(() => PlayerItems.Add(playerItem));

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
