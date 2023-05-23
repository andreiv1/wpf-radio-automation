using CommunityToolkit.Mvvm.ComponentModel;
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
using System.Runtime.Serialization;
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


        [ObservableProperty]
        private IPlayerItem? selectedPlaylistItem;
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

            playbackQueue.Mode = PlaybackMode.Manual;
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
                        PlaybackAddItem(new TrackPlaylistPlayerItem(trackDto));
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

            if (MainVm != null)
            {

                playbackQueue.UpdateETAs(MainVm.NowPlayingVm?.RemainingNow ?? null);
            }
            else
            {
                playbackQueue.UpdateETAs(null);
            }
        }
        public void PlaybackAddItem(IPlayerItem playerItem, int position)
        {
            playbackQueue.AddItem(playerItem, position);
            dispatcherService.InvokeOnUIThread(() => PlayerItems.Insert(position,playerItem));

            if (MainVm != null)
            {
 
                playbackQueue.UpdateETAs(MainVm.NowPlayingVm?.RemainingNow ?? null);
            }
            else
            {
                playbackQueue.UpdateETAs(null);
            }
        }

        public void PlaybackRemoveItem(IPlayerItem playerItem)
        {
            playbackQueue.RemoveItem(playerItem);
            dispatcherService.InvokeOnUIThread(() => PlayerItems.Remove(playerItem));

            if (MainVm != null)
            {

                playbackQueue.UpdateETAs(MainVm.NowPlayingVm?.RemainingNow ?? null);
            }
            else
            {
                playbackQueue.UpdateETAs(null);
            }
        }

        public void PlaybackMoveItem(IPlayerItem playerItem, int index)
        {
            if (index < 0 || index >= PlayerItems.Count) { return; }
            playbackQueue.MoveItem(playerItem, index);
            PlayerItems.Remove(playerItem);
            PlayerItems.Insert(index, playerItem);
            SelectedPlaylistItem = PlayerItems.ElementAt(index);
            playbackQueue.UpdateETAs(MainVm.NowPlayingVm.RemainingNow ?? null);
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
                MainVm.NowPlayingVm.UpdateNowPlaying(playerItemNow.Artists ?? "", playerItemNow.Title, playerItemNow.Duration, 
                    playerItemNow.ImagePath);
            }
        }

        private System.Timers.Timer timer;
        private void CalculateRemaining()
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }

            timer = new System.Timers.Timer(1000);
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
            timer.Start();
        }

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if (playbackQueue.NowPlaying != playerItemNow)
            {
                timer.Stop();
                timer.Dispose();
                return;
            }
            MainVm.NowPlayingVm.ElapsedNow += TimeSpan.FromSeconds(1);
            MainVm.NowPlayingVm.RemainingNow = MainVm.NowPlayingVm.DurationNow - MainVm.NowPlayingVm.ElapsedNow;
        }

        #region Commands
        [RelayCommand]
        private void PlayNext()
        {
            MainVm.NowPlayingVm.IsPaused = false;
            MainVm.NowPlayingVm.IsItemLoaded = true;
            playbackQueue.Stop();
            playbackQueue.Play();
            
            CalculateRemaining();

        }

        [RelayCommand]
        private void Loop()
        {

        }

        [RelayCommand]
        private void Pause()
        {
            playbackQueue.Pause();
            CalculateRemaining();
        }

        [RelayCommand]
        private void Stop()
        {
            playbackQueue.Stop();
            CalculateRemaining();
        }

        [RelayCommand]
        private void Restart()
        {

        }

        [RelayCommand]
        private void AddTrackToTop()
        {
            if (MainVm.MediaItemsVm.SelectedTrack == null) return;
            IPlayerItem newItem = new TrackListingPlayerItem(MainVm.MediaItemsVm.SelectedTrack);
            PlaybackAddItem(newItem, 0);
            SelectedPlaylistItem = newItem;
        }

        [RelayCommand]
        private void AddTrackToBottom()
        {
            if (MainVm.MediaItemsVm.SelectedTrack == null) return;
            IPlayerItem newItem = new TrackListingPlayerItem(MainVm.MediaItemsVm.SelectedTrack);
            PlaybackAddItem(newItem);
            SelectedPlaylistItem = newItem;
        }

        [RelayCommand]
        private void InsertTrack()
        {
            if(SelectedPlaylistItem == null) return;
            if (MainVm.MediaItemsVm.SelectedTrack == null) return;
            int index = PlayerItems.IndexOf(SelectedPlaylistItem) + 1;
            IPlayerItem newItem = new TrackListingPlayerItem(MainVm.MediaItemsVm.SelectedTrack);
            PlaybackAddItem(newItem, index);
            SelectedPlaylistItem = newItem;
        }

        [RelayCommand]
        private void ReplaceTrack()
        {
            if (SelectedPlaylistItem == null) return;
            if (MainVm.MediaItemsVm.SelectedTrack == null) return;
            int originalIndex = PlayerItems.IndexOf(SelectedPlaylistItem);
            PlaybackRemoveItem(SelectedPlaylistItem);
            IPlayerItem newItem = new TrackListingPlayerItem(MainVm.MediaItemsVm.SelectedTrack);
            PlaybackAddItem(newItem, originalIndex);
            SelectedPlaylistItem = newItem;
        }

        [RelayCommand]
        private void DeleteTrack(object parameter)
        {
            IPlayerItem? playerItem = parameter as IPlayerItem;
            if (playerItem == null) return;
            PlaybackRemoveItem(playerItem);
        }

        [RelayCommand]
        private void MoveTrackUp(object parameter)
        {
            IPlayerItem? playerItem = parameter as IPlayerItem;
            if (playerItem == null) return;
            int index = PlayerItems.IndexOf(playerItem);
            PlaybackMoveItem(playerItem, --index);
        }

        [RelayCommand]
        private void MoveTrackDown(object parameter)
        {
            IPlayerItem? playerItem = parameter as IPlayerItem;
            if (playerItem == null) return;
            int index = PlayerItems.IndexOf(playerItem);
            PlaybackMoveItem(playerItem, ++index);
        }

        [RelayCommand]
        private void ClearAllTracks()
        {

        }
       
        #endregion
    }

}
