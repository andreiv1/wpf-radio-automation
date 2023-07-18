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

        public RandomTrackSelectionStrategy(IDbContextFactory<AppDbContext> dbContextFactory,
                                            TrackSelectionOptions options,
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
                .ToList();

            List<int>? recentlyPlayedTrackIds = lastTracks?.Where(i => i.ETA > item.ETA.AddMinutes(-options.TrackSeparation.Value))
                .Select(it => it.Track?.Id ?? 0)
                .ToList();

            List<List<int>?>? recentlyPlayedArtistIdsByTrack = lastTracks?.Where(i => i.ETA > item.ETA.AddMinutes(-options.ArtistSeparation.Value))
                .Select(it => it.Track?.ArtistsId ?? null)
                .ToList();

            HashSet<int> recentlyPlayedArtists = new HashSet<int>();
            if (recentlyPlayedArtistIdsByTrack != null)
            {
                foreach (var artistIds in recentlyPlayedArtistIdsByTrack)
                {
                    if(artistIds != null)
                        foreach (var artistId in artistIds)
                        {
                            recentlyPlayedArtists.Add(artistId);
                        }
                }
            }

            var track = GetRandomTrack(categoryId, recentlyPlayedTrackIds, recentlyPlayedArtists).Result;
            if (track != null)
            {
                item.Length = track.Duration;
                item.Track = track;
            }
            return item;
        }

        private static readonly Random random = new Random();
        public async Task<TrackListingDTO?> GetRandomTrack(int categoryId,
                                                           List<int>? trackIdsToExclude = null,
                                                           HashSet<int>? artistIdsToExclude = null)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();

            var noOfTracksQuery = dbContext.GetTracksByCategoryId(categoryId)
                .AsQueryable();

            var query = dbContext.GetTracksByCategoryId(categoryId).Include(t => t.Categories)
                .Include(t => t.TrackArtists)
                .ThenInclude(ta => ta.Artist)
                .Include(t => t.TrackTags)
                .AsQueryable();

            if (trackIdsToExclude != null)
            {
                query = query.Where(t => !trackIdsToExclude.Contains(t.Id));
                noOfTracksQuery = noOfTracksQuery.Where(t => !trackIdsToExclude.Contains(t.Id));
            }

            if(artistIdsToExclude != null)
            {
                query = query.Where(t => !t.TrackArtists.Any(ta => artistIdsToExclude.Contains(ta.ArtistId)));
                noOfTracksQuery = noOfTracksQuery.Where(t => !t.TrackArtists.Any(ta => artistIdsToExclude.Contains(ta.ArtistId)));
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
