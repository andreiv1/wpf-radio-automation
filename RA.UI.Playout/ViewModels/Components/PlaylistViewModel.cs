using CommunityToolkit.Mvvm.Input;
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

            playbackQueue.PlaybackStarted += PlaybackQueue_PlaybackStarted;

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

        #region Playback Queue events & logic 
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
        private void PlaybackQueue_PlaybackStarted(object? sender, EventArgs e)
        {
            if (PlayerItems.ElementAt(0) is not null)
            {
                playerItemNow = PlayerItems.ElementAt(0);
                PlayerItems.RemoveAt(0);

                UpdateNowPlaying();
            }
        }
        #endregion

        private void UpdateNowPlaying()
        {
            if (playerItemNow is not null)
            {
                MainVm.NowPlayingVm.UpdateNowPlaying(playerItemNow.Artists ?? "", playerItemNow.Title, playerItemNow.Duration);
            }
        }

        #region Commands
        [RelayCommand]
        private void PlayNext()
        {
            MainVm.NowPlayingVm.IsPaused = false;
            MainVm.NowPlayingVm.IsItemLoaded = true;
            playbackQueue.Play();
            //CalculateRemaining();

        }

        #endregion
    }

}
