using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using RA.Database;
using RA.Dto;
using RA.Logic.AudioPlayer;
using RA.Logic.AudioPlayer.Interfaces;
using RA.DatabaseModels;
using RA.UI.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.Playout.ViewModels.Controls
{
    public partial class PlaylistViewModel : ViewModelBase
    {
        private IPlaybackQueue playbackQueue;

        private IPlayerItem playerItemNow;
        private readonly NowPlayingViewModel nowPlayingVm;

        public MainViewModel MainVm { get; set; }

        [ObservableProperty]
        private bool isAutoPlayOn = false;

        partial void OnIsAutoPlayOnChanged(bool value)
        {
            playbackQueue.Mode = value ? PlaybackMode.Auto : PlaybackMode.Manual;
        }

        [ObservableProperty]
        private bool isAutoReloadOn = false;

        [ObservableProperty]
        private bool isPaused = false;

        [ObservableProperty]
        private bool isLooping = false;

        partial void OnIsLoopingChanged(bool value)
        {
            if (value)
            {
                playbackQueue.Mode = PlaybackMode.Loop;
            }
            else
            {
                if (IsAutoPlayOn)
                {
                    playbackQueue.Mode = PlaybackMode.Auto;
                }
                else
                {
                    playbackQueue.Mode = PlaybackMode.Manual;
                }
            }
        }

        public ObservableCollection<IPlayerItem> PlayerItems { get; set; } = new();

        [ObservableProperty]
        private IPlayerItem selectedPlaylistItem;

        #region Constructor
        public PlaylistViewModel(NowPlayingViewModel nowPlayingVm, IPlaybackQueue playbackQueue)
        {
            this.nowPlayingVm = nowPlayingVm;
            this.playbackQueue = playbackQueue;
            playbackQueue.PlaybackStarted += OnPlayback_Started;
            playbackQueue.PlaybackItemChange += OnPlaybackItem_Change;
            playbackQueue.PlaybackStopped += OnPlayback_Stopped;
            LoadCurrentHourPlaylist();
        }
        #endregion

        private void LoadCurrentHourPlaylist(DateTime? dateNow = null)
        {
            if (dateNow == null)
            {
                dateNow = DateTime.Now;
            }

            try
            {
                using (var db = new AppDbContext())
                {
                    var playlistToAir = db.Playlists
                        .Include(p => p.PlaylistItems)
                        .ThenInclude(pi => pi.Track)
                        .ThenInclude(t => t.TrackArtists)
                        .ThenInclude(a => a.Artist)
                        .Where(p => p.AirDate.Date == dateNow.Value.Date).FirstOrDefault();

                    if (playlistToAir is not null)
                    {
                        var startHour = new DateTime(dateNow.Value.Year, dateNow.Value.Month, dateNow.Value.Day, dateNow.Value.Hour, dateNow.Value.Minute, 0);
                        var endHour = RoundDate(startHour);

                        List<PlaylistItemDto> playlistHourItems = db.PlaylistItems
                            .Include(pi => pi.Track)
                            .ThenInclude(t => t.TrackArtists)
                            .ThenInclude(a => a.Artist)
                            .Where(pi => pi.PlaylistId == playlistToAir.Id &&
                                    pi.ETA >= startHour && pi.ETA <= endHour)
                            .Select(pi => PlaylistItemDto.FromEntity(pi))
                            .ToList();

                        foreach (var item in playlistHourItems)
                        {
                            Console.WriteLine($"{item.ETA} - {item.Id}");
                            IPlayerItem playerItem = new PlayerItem(item);
                            PlaybackAddItem(playerItem);
                        }

                    }
                    else
                    {
                        throw new Exception($"Playlist for date {dateNow.Value.Date.Date.ToString("dd/MM/yyyy HH:mm")} not found!");
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        #region Playback queue

        public void PlaybackAddItem(IPlayerItem playerItem)
        {
            playbackQueue.AddItem(playerItem);
            PlayerItems.Add(playerItem);
            if (MainVm is not null)
            {
                playbackQueue.UpdateETAs(MainVm.NowPlayingVm?.RemainingNow ?? null);
            } else
            {
                playbackQueue.UpdateETAs(null);
            }
        }

        public void PlaybackRemoveItem(IPlayerItem playerItem)
        {
            playbackQueue.RemoveItem(playerItem);
            PlayerItems.Remove(playerItem);
            playbackQueue.UpdateETAs(MainVm.NowPlayingVm?.RemainingNow ?? null);
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

        private void OnPlayback_Stopped(object? sender, EventArgs e)
        {
            nowPlayingVm.Reset();
            if(playbackQueue.NowPlaying is not null)
            {
                UpdateNowPlaying();
                CalculateRemaining();
            }
        }

        private void OnPlaybackItem_Change(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnPlayback_Started(object? sender, EventArgs e)
        {
            if (PlayerItems.ElementAt(0) is not null
                && !IsLooping)
            {
                playerItemNow = PlayerItems.ElementAt(0);
                PlayerItems.RemoveAt(0);
            }
            UpdateNowPlaying();
            CalculateRemaining();
        }

        private void UpdateNowPlaying()
        {
            if (playerItemNow is not null)
            {
                nowPlayingVm.UpdateNowPlaying(playerItemNow.Artists ?? "", playerItemNow.Title, playerItemNow.Duration);
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
            nowPlayingVm.ElapsedNow += TimeSpan.FromSeconds(1);
            nowPlayingVm.RemainingNow = nowPlayingVm.DurationNow - nowPlayingVm.ElapsedNow;
        }
        #endregion

        #region Commands
        [RelayCommand]
        private void Play()
        {
            nowPlayingVm.IsPaused = false;
            nowPlayingVm.IsItemLoaded = true;
            playbackQueue.Play();
            CalculateRemaining();
           
        }

        [RelayCommand]
        private void Stop()
        {
            playbackQueue.Stop();
            nowPlayingVm.Reset();
        }

        [RelayCommand]
        private void Pause()
        {
            playbackQueue.Pause();
            nowPlayingVm.IsPaused = true;
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }
        }

        [RelayCommand]
        private void Reload()
        {
            Clear();
           
            LoadCurrentHourPlaylist();
            if (playerItemNow is not null)
            {
                playbackQueue.UpdateETAs(playerItemNow.Duration);
            }
        }

        [RelayCommand]
        private void Resume()
        {
            nowPlayingVm.IsPaused = false;
            nowPlayingVm.IsItemLoaded = true;
            playbackQueue.Resume();
            CalculateRemaining();
        }

        [RelayCommand]
        private void Clear()
        {
            PlayerItems.Clear();
            playbackQueue.ClearQueue();
        }

        [RelayCommand]
        private void RemovePlaylistItem(object parameter)
        {
            IPlayerItem? playerItem = parameter as IPlayerItem;
            if(playerItem is not null)
            {
                PlaybackRemoveItem(playerItem);
            }
        }

        [RelayCommand]
        private void MoveSelectedItemDown()
        {
            if(SelectedPlaylistItem is not null)
            {
                int index = PlayerItems.IndexOf(SelectedPlaylistItem);
                PlaybackMoveItem(SelectedPlaylistItem, ++index);
            }
        }

        [RelayCommand]
        private void MoveSelectedItemUp()
        {
            if (SelectedPlaylistItem is not null)
            {
                int index = PlayerItems.IndexOf(SelectedPlaylistItem);
                PlaybackMoveItem(SelectedPlaylistItem, --index);
            }
        }

        [RelayCommand]
        private void InsertSelectedLibraryItem()
        {
            if(SelectedPlaylistItem is not null)
            {
                int index = PlayerItems.IndexOf(SelectedPlaylistItem) + 1;
                throw new NotImplementedException();
            }
        }
        #endregion

        static DateTime RoundDate(DateTime inputDate)
        {
            return new DateTime(inputDate.Year, inputDate.Month, inputDate.Day, inputDate.Hour, 0, 0).AddHours(1);
        }
    }
}
