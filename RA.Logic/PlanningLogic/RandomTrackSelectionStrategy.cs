using RA.DAL;
using RA.DTO;
using RA.Logic.PlanningLogic.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RA.Logic.PlanningLogic
{
    public class RandomTrackSelectionStrategy : TrackSelectionBaseStrategy
    {
        private readonly int categoryId;

        //Generation works only with a fixed sep
        public RandomTrackSelectionStrategy(IPlaylistsService playlistsService, ITracksService tracksService, 
            int categoryId,
            int artistSeparation = 30, int trackSeparation = 60, int titleSeparation = 30) 
            : base(playlistsService, tracksService, artistSeparation, trackSeparation, titleSeparation)
        {
            this.categoryId = categoryId;
        }

        public override PlaylistItemTrackDTO SelectTrack(PlaylistDTO playlist)
        {
            var lastItem = playlist.Items?.LastOrDefault();
            PlaylistItemTrackDTO item = new();
            if (lastItem == null)
            {
                item.ETA = new DateTime(playlist.AirDate.Year, playlist.AirDate.Month, playlist.AirDate.Day);
            } else
            {
                item.ETA = lastItem.ETA + TimeSpan.FromSeconds(lastItem.Length);
            }

            var lastTracks = playlist!.Items!
                .Where(it => it.GetType() == typeof(PlaylistItemTrackDTO))
                .Select(it => (PlaylistItemTrackDTO)it).ToList();

            List<int> recentlyPlayedTrackIds = lastTracks.Where(i => i.ETA > item.ETA.AddMinutes(-trackSeparation))
                .Select(it => it.TrackId)
                .ToList();

            var track = tracksService.GetRandomTrack(categoryId, recentlyPlayedTrackIds).Result;
            Console.WriteLine($"[{item.ETA}] {track.Id} - {track.Artists} - {track.Title}");
            item.TrackId = track.Id;
            item.Length = track.Duration;

            playlist?.Items?.Add(item);
            return item;
        }
    }
}
