using RA.DAL;
using RA.DTO;
using RA.Logic.Planning.Abstract;

namespace RA.Logic.Planning
{
    public class RandomTrackSelectionStrategy : TrackSelectionBaseStrategy
    {
        private readonly int categoryId;
        public RandomTrackSelectionStrategy(IPlaylistsService playlistsService,
                                            ITracksService tracksService,
                                            int categoryId,
                                            int artistSeparation,
                                            int trackSeparation,
                                            int titleSeparation) 
            : base(playlistsService, tracksService, artistSeparation, trackSeparation, titleSeparation)
        {
            this.categoryId = categoryId;
        }

        public override PlaylistItemTrackDTO SelectTrack(PlaylistDTO playlist)
        {
            if (playlist == null) throw new ArgumentNullException($"{nameof(playlist)} must be initialised.");
            var lastItem = playlist.Items?.LastOrDefault();
            PlaylistItemTrackDTO item = new();
            if (lastItem == null)
            {
                item.ETA = new DateTime(playlist.AirDate.Year, playlist.AirDate.Month, playlist.AirDate.Day);
            } else
            {
                item.ETA = lastItem.ETA + TimeSpan.FromSeconds(lastItem.Length);
            }

            var lastTracks = playlist.Items?
                .Where(it => it.GetType() == typeof(PlaylistItemTrackDTO))
                .Select(it => (PlaylistItemTrackDTO)it).ToList();

            List<int>? recentlyPlayedTrackIds = lastTracks?.Where(i => i.ETA > item.ETA.AddMinutes(-trackSeparation))
                .Select(it => it.Track.Id)
                .ToList();

            var track = tracksService.GetRandomTrack(categoryId, recentlyPlayedTrackIds).Result;
            
            if (track != null)
            {
                playlist?.Items?.Add(item);
                item.Length = track.Duration;
                item.Track = track;
            }
            return item;
        }
    }
}
