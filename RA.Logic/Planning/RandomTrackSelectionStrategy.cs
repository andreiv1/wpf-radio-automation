using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using RA.Database;
using RA.Database.Models;
using RA.DTO;
using RA.Logic.Planning.Abstract;

namespace RA.Logic.Planning
{
    public class RandomTrackSelectionStrategy : TrackSelectionBaseStrategy
    {
        private readonly int categoryId;

        public RandomTrackSelectionStrategy(IDbContextFactory<AppDbContext> dbContextFactory, TrackSelectionOptions options,
            int categoryId) : base(dbContextFactory, options)
        {
            this.categoryId = categoryId;
        }

        public override PlaylistItemDTO SelectTrack(PlaylistDTO currentPlaylist)
        {
            if (currentPlaylist == null) throw new ArgumentNullException($"{nameof(currentPlaylist)} must be initialised.");
            if (options == null || options.TrackSeparation == null || options.ArtistSeparation == null || options.TitleSeparation == null)
                throw new ArgumentNullException($"{nameof(options.TrackSeparation)},{nameof(options.ArtistSeparation)},{nameof(options.TitleSeparation)} must be initialised.");
            var lastItem = currentPlaylist.Items?.LastOrDefault();
            PlaylistItemDTO item = new();
            if (lastItem == null)
            {
                item.ETA = new DateTime(currentPlaylist.AirDate.Year, currentPlaylist.AirDate.Month, currentPlaylist.AirDate.Day);
            }
            else
            {
                item.ETA = lastItem.ETA + TimeSpan.FromSeconds(lastItem.Length);
            }

            var lastTracks = currentPlaylist.Items?
                .Where(it => it.GetType() == typeof(PlaylistItemDTO))
                .Select(it => (PlaylistItemDTO)it).ToList();

            List<int>? recentlyPlayedTrackIds = lastTracks?.Where(i => i.ETA > item.ETA.AddMinutes(-options.TrackSeparation.Value))
                .Select(it => it.Track.Id)
                .ToList();

            var track = GetRandomTrack(categoryId, recentlyPlayedTrackIds).Result;
            if (track != null)
            {
                currentPlaylist?.Items?.Add(item);
                item.Length = track.Duration;
                item.Track = track;
            }
            return item;
        }

        private static readonly Random random = new Random();
        public async Task<TrackListingDTO?> GetRandomTrack(int categoryId, List<int>? trackIdsToExclude = null)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();

            var noOfTracksQuery = dbContext.GetTracksByCategoryId(categoryId)
                .AsQueryable();

            var query = dbContext.GetTracksByCategoryId(categoryId).Include(t => t.Categories)
                .Include(t => t.TrackArtists)
                .ThenInclude(ta => ta.Artist)
                .AsQueryable();

            if (trackIdsToExclude != null)
            {
                query = query.Where(t => !trackIdsToExclude.Contains(t.Id));
                noOfTracksQuery = noOfTracksQuery.Where(t => !trackIdsToExclude.Contains(t.Id));
            }

            var noOfTracks = await noOfTracksQuery.CountAsync();
            var track = await query.Skip(random.Next(noOfTracks))
                .Take(1)
                .Select(t => TrackListingDTO.FromEntity(t))
                .FirstOrDefaultAsync();
            return track;
        }
    }
}
