using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using RA.Database;
using RA.Database.Models;
using RA.DTO;
using System.Diagnostics;

namespace RA.DAL
{
    public class TracksService : ITracksService
    {
        private readonly IDbContextFactory<AppDbContext> dbContextFactory;

        public TracksService(IDbContextFactory<AppDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task<int> GetTrackCountAsync(string query = "", bool includeDisabled = false)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                return await dbContext
                    .GetTracks(query, includeDisabled)
                    .AsNoTracking()
                    .CountAsync();
            }
        }

        public async Task<IEnumerable<TrackListingDTO>> GetTrackListAsync(int skip, int take, string query = "", bool includeDisabled = false)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
                return await dbContext.GetTracks(query, includeDisabled)
                    .Skip(skip)
                    .Take(take)
                    .AsNoTracking()
                    .Select(t => TrackListingDTO.FromEntity(t))
                    .ToListAsync();
            
        }

        public IEnumerable<TrackListingDTO> GetTrackList(int skip, int take)
        {
            return GetTrackListAsync(skip, take).Result;
        }

        public async Task<TrackDTO> GetTrack(int id)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            return await dbContext.GetTrackById(id)
                .Include(c => c.Categories)
                .Include(tt => tt.TrackTags)
                .ThenInclude(tt => tt.TagValue)
                .AsNoTracking()
                .Select(t => TrackDTO.FromEntity(t))
                .FirstAsync();
        }

        public async Task UpdateTrack(TrackDTO trackDTO)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var existingTrack = dbContext.Tracks
                .Where(t => t.Id == trackDTO.Id)
                .Include(t => t.Categories)
                .Include(t => t.TrackTags)
                .FirstOrDefault();

            var existingAddedDate = existingTrack.DateAdded;
            var track = TrackDTO.ToEntity(trackDTO);
            if (existingTrack == null) return;

            dbContext.Entry(existingTrack).CurrentValues.SetValues(track);
            existingTrack.DateAdded = existingAddedDate;
            existingTrack.DateModified = DateTime.Now;

            if (track.Categories != null)
            {
                // Remove categories that are no longer associated with the track
                foreach (var existingCategory in existingTrack.Categories.ToList())
                {
                    if (!track.Categories
                        .Any(c => c.Id == existingCategory.Id))
                    {
                        existingTrack.Categories.Remove(existingCategory);
                    }
                }

                // Add new categories to the track
                foreach (var newCategory in track.Categories)
                {
                    if (!existingTrack.Categories
                        .Any(c => c.Id == newCategory.Id))
                    {
                        existingTrack.Categories.Add(newCategory);
                    }
                }
            }

            if (track.TrackTags != null)
            {
                // Remove tags that are no longer associated
                foreach (var existingTag in existingTrack.TrackTags.ToList())
                {
                    if (!track.TrackTags
                        .Any(tt => tt.TagValueId == existingTag.TagValueId))
                    {
                        existingTrack.TrackTags.Remove(existingTag);
                    }
                }
                // Add new tags to the track
                foreach (var newTag in track.TrackTags)
                {
                    if (!existingTrack.TrackTags
                        .Any(tt => tt.TagValueId == newTag.TagValueId))
                    {
                        existingTrack.TrackTags.Add(newTag);
                    }
                }
            }

            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteTrack(int trackId)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var isDeleted = false;
            try
            {
                var track = await dbContext.Tracks.FirstOrDefaultAsync(t => t.Id == trackId);
                if (track != null)
                {
                    track.DateDeleted = DateTime.Now;
                    await dbContext.SaveChangesAsync();
                    isDeleted = true;
                }
            }
            catch (DbUpdateException e)
            {
                isDeleted = false;
                Debug.WriteLine($"Error deleting TrackId={trackId}: {e.Message}");
            }

            return isDeleted;
        }

        public async Task<IEnumerable<TrackListingDTO>> GetTrackListByArtistAsync(int artistId, int skip, int take)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            //return await dbContext.GetTracks()
            //    .Skip(skip).Take(take)
            //    .Where(t => t.TrackArtists.Contains(new ArtistTrack
            //    {
            //        ArtistId = artistId,
            //        TrackId = t.Id,
            //    }))
            //    .Select(t => TrackListingDTO.FromEntity(t))
            //    .ToListAsync();
            //TODO: Bug
            var result = dbContext.GetTracks()
                .Skip(skip).Take(take)
                .Where(t => t.TrackArtists.Any(ta => ta.ArtistId == artistId))
                .Select(t => TrackListingDTO.FromEntity(t));

            return await result.ToListAsync();
        }

        public IEnumerable<TrackListingDTO> GetTrackListByArtist(int artistId, int skip, int take)
        {
            return GetTrackListByArtistAsync(artistId, skip, take).Result;
        }

        public async Task<IEnumerable<TrackListingDTO>> GetTrackListByCategoryAsync(int categoryId, int skip, int take)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var result = await dbContext.GetTracks()
               .Include(c => c.Categories)
               .Where(t => t.Categories.Any(x => x.Id == categoryId))
               .Skip(skip).Take(take)
               .Select(t => TrackListingDTO.FromEntity(t))
               .AsNoTracking()
               .ToListAsync();

            return result;
        }

        public async Task<int> GetTrackCountByCategoryAsync(int categoryId)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            return await dbContext.GetTracks()
                .Include(c => c.Categories)
                .Where(t => t.Categories.Contains(new Category()
                {
                    Id = categoryId,
                }))
                .CountAsync();
        }

        public async Task<int> AddTracks(IEnumerable<TrackDTO> trackDTOs)
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var tracks = trackDTOs.Select(t => TrackDTO.ToEntity(t)).ToList();
            foreach(var t in tracks)
            {
                var categories = t.Categories.ToList();
                for(int i = 0; i < categories.Count; i++)
                {
                    categories[i] = dbContext.AttachOrGetTrackedEntity<Category>(categories[i]);
                }
                t.Categories = categories;
            }
            dbContext.Tracks.AddRange(tracks);
            await dbContext.SaveChangesAsync();
            return tracks.Count;
        }

        public async Task<bool> TrackExistsByPath(String filePath)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var query = dbContext.Tracks
                .Where(t => t.FilePath == filePath)
                .Select(t => t.FilePath);
            return await query.AnyAsync();
        }

        private static readonly Random random = new Random();
        public async Task<TrackListingDTO?> GetRandomTrack(int categoryId, List<int>? trackIdsToExclude = null)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();

            var noOfTracksQuery = dbContext.Tracks.Include(t => t.Categories)
                .Where(t => t.Categories.Contains(new Category() { Id = categoryId }))
                .AsQueryable();

            var query = dbContext.Tracks.Include(t => t.Categories)
                .Include(t => t.TrackArtists)
                .ThenInclude(ta => ta.Artist)
                .Where(t => t.Categories.Contains(new Category() { Id = categoryId }));

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
