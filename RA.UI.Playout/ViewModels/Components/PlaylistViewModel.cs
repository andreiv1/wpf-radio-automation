using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.DAL;
using RA.Database.Models.Enums;
using RA.DTO;
using RA.Logic;
using RA.Logic.AudioPlayer.Interfaces;
using RA.UI.Core.Services;
using RA.UI.Core.ViewModels;
using RA.UI.Playout.ViewModels.Components.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RA.UI.Playout.ViewModels.Components
{
    public partial class PlaylistViewModel : ViewModelBase
    {
        private readonly IDispatcherService dispatcherService;
        private readonly IPlaybackQueue playbackQueue;
        private readonly IPlaylistsService playlistsService;
        private readonly ITrackHistoryService trackHistoryService;
        private IPlayerItem? playerItemNow;

        [ObservableProperty]
        private IPlayerItem? selectedPlaylistItem;

        [ObservableProperty]
        private String loadedPlaylistDuration = "00:00:00";

        [ObservableProperty]
        private bool isAutoPlay = false;

        [ObservableProperty]
        private bool isAutoReload = true;

        partial void OnIsAutoPlayChanged(bool value)
        {
            if(value)
            {
                playbackQueue.Mode = PlaybackMode.Auto;
                if (playbackQueue.NowPlaying == null)
                {
                    Play();
                }
            } else
            {
                playbackQueue.Mode = PlaybackMode.Manual;
            }
        }
        public MainViewModel? MainVm { get; set; }
        public ObservableCollection<IPlayerItem> PlayerItems { get; } = new();
        public PlaylistViewModel(IDispatcherService dispatcherService,
                                 IPlaybackQueue playbackQueue,
                                 IPlaylistsService playlistsService,
                                 ITrackHistoryService trackHistoryService)
        {
            this.dispatcherService = dispatcherService;
            this.playbackQueue = playbackQueue;
            this.playlistsService = playlistsService;
            this.trackHistoryService = trackHistoryService;
            playbackQueue.PlaybackStarted += PlaybackQueue_PlaybackStarted;
            playbackQueue.PlaybackStopped += PlaybackQueue_PlaybackStopped;
            playbackQueue.PlaybackItemChange += PlaybackQueue_PlaybackItemChange;

            PlayerItems.CollectionChanged += PlayerItems_CollectionChanged;

            _ = LoadPlaylist(DateTime.Now,1);
        }

        private void PlaybackQueue_PlaybackStopped(object? sender, EventArgs e)
        {
            
        }

        private void PlayerItems_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            TimeSpan totalDuration = TimeSpan.Zero;
            foreach(var item in PlayerItems)
            {
               IPlayerItem? playerItem = item;
               if(playerItem != null)
               {
                  totalDuration += playerItem.Duration;
               }
            }
            LoadedPlaylistDuration = totalDuration.ToString(@"hh\:mm\:ss");
        }

        private void PlaybackQueue_PlaybackItemChange(object? sender, EventArgs e)
        {
            DebugHelper.WriteLine(this, $"Playback item changed");
        }
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
            if(MainVm != null)
                playbackQueue.UpdateETAs(MainVm.NowPlayingVm.RemainingNow ?? null);
        }

        public void PlaybackClearItems()
        {
            PlayerItems.Clear();
            playbackQueue.ClearQueue();
        }
        private void PlaybackQueue_PlaybackStarted(object? sender, EventArgs e)
        {
            if (PlayerItems.ElementAt(0) is not null)
            {
                playerItemNow = PlayerItems.ElementAt(0);
                PlayerItems.RemoveAt(0);

                UpdateNowPlaying();
                AddNowToHistory();
            }
        }
        private void UpdateNowPlaying()
        {
            if (playerItemNow != null)
            {
                MainVm!.NowPlayingVm.UpdateNowPlaying(artist: playerItemNow.Artists ?? "", 
                                                     title: playerItemNow.Title, 
                                                     duration: playerItemNow.Duration, 
                                                     image: playerItemNow.ImagePath);
            }
        }

        private CancellationTokenSource? cancellationTokenSource = null;
        private void CalculateRemaining()
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource?.Cancel();
                cancellationTokenSource?.Dispose();
                cancellationTokenSource = null;
            }

            cancellationTokenSource = new CancellationTokenSource();
            _ = StartTimerAsync(cancellationTokenSource.Token);
        }

        private async Task StartTimerAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1000, cancellationToken);

                if (playbackQueue.NowPlaying != playerItemNow)
                {
                    cancellationTokenSource?.Cancel();
                    cancellationTokenSource?.Dispose();
                    cancellationTokenSource = null;
                    return;
                }

                MainVm!.NowPlayingVm.ElapsedNow += TimeSpan.FromSeconds(1);
                MainVm!.NowPlayingVm.RemainingNow = MainVm.NowPlayingVm.DurationNow - MainVm.NowPlayingVm.ElapsedNow;
            }
        }

        private void Play()
        {
            MainVm!.NowPlayingVm.IsPaused = false;
            MainVm!.NowPlayingVm.IsItemLoaded = true;
            playbackQueue.Play();
            CalculateRemaining();
        }

        private void AddNowToHistory()
        {
            TrackType trackType = (TrackType)Enum.Parse(typeof(TrackType), playerItemNow.TrackType);
            var trackHistory = new TrackHistoryDTO()
            {
                DatePlayed = DateTime.Now,
                TrackId = playerItemNow.TrackId,
                TrackType = trackType,
            };
            
            var addTask = trackHistoryService.AddTrackToHistory(trackHistory);
            addTask.ContinueWith(async (t) => await MainVm!.HistoryVm.AddItem(trackHistory.DatePlayed));
        }

        [RelayCommand]
        private void Next()
        {

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
            MainVm!.NowPlayingVm.Reset();
        }

        [RelayCommand]
        private void Restart()
        {
            if(playerItemNow != null)
            {
                Seek(new TimeSpan[] { TimeSpan.Zero, playerItemNow.Duration });
                UpdateNowPlaying();
            }
            
        }

        [RelayCommand]
        private void Seek(TimeSpan[] param)
        {
            TimeSpan position = param[0];
            TimeSpan remaining = param[1];
            playbackQueue.Seek(position);
            playbackQueue.UpdateETAs(remaining);
        }

        [RelayCommand]
        private void AddTrackToTop()
        {
            if (MainVm!.MediaItemsVm.SelectedTrack == null) return;
            IPlayerItem newItem = new TrackListingPlayerItem(MainVm.MediaItemsVm.SelectedTrack);
            PlaybackAddItem(newItem, 0);
            SelectedPlaylistItem = newItem;
        }

        [RelayCommand]
        private void AddTrackToBottom()
        {
            if (MainVm!.MediaItemsVm.SelectedTrack == null) return;
            IPlayerItem newItem = new TrackListingPlayerItem(MainVm.MediaItemsVm.SelectedTrack);
            PlaybackAddItem(newItem);
            SelectedPlaylistItem = newItem;
        }

        [RelayCommand]
        private void InsertTrack()
        {
            if(SelectedPlaylistItem == null) return;
            if (MainVm!.MediaItemsVm.SelectedTrack == null) return;
            int index = PlayerItems.IndexOf(SelectedPlaylistItem) + 1;
            IPlayerItem newItem = new TrackListingPlayerItem(MainVm.MediaItemsVm.SelectedTrack);
            PlaybackAddItem(newItem, index);
            SelectedPlaylistItem = newItem;
        }

        [RelayCommand]
        private void ReplaceTrack()
        {
            if (SelectedPlaylistItem == null) return;
            if (MainVm!.MediaItemsVm.SelectedTrack == null) return;
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
        private void ClearPlaylist()
        {
            PlaybackClearItems();
        }
    
    }

}
