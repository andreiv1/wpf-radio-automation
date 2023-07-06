using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using RA.DAL.Models;
using RA.Database;
using RA.Database.Models;
using RA.Database.Models.Enums;
using RA.DTO;
using System.Diagnostics;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace RA.DAL
{
    public class TracksService : ITracksService
    {
        private readonly IDbContextFactory<AppDbContext> dbContextFactory;

        public TracksService(IDbContextFactory<AppDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task<int> GetTrackCountAsync(string searchQuery = "", ICollection<TrackFilterCondition>? conditions = null)
        {
            using var dbContext = dbContextFactory.CreateDbContext();

            var query = dbContext
                .GetTracks(searchQuery)
                .IgnoreQueryFilters()
                .AsNoTracking();

            if (conditions != null)
            {
                var filters = GenerateFilters(conditions);

                foreach (var filter in filters)
                {
                    query = query.Where(filter.Value);
                }
            }

            return await query.CountAsync();

        }

        public async Task<IEnumerable<TrackListingDTO>> GetTrackListAsync(int skip,
                                                                          int take,
                                                                          string searchQuery = "",
                                                                          ICollection<TrackFilterCondition>? conditions = null)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var query = dbContext.GetTracks(searchQuery)
                    .Include(t => t.Categories)
                    .AsNoTracking();

            if (conditions != null)
            {
                var filters = GenerateFilters(conditions);
                foreach (var filter in filters)
                {
                    query = query.Where(filter.Value);
                }
            }

            query = query
                .OrderBy(t => t.Id)
                .Skip(skip).Take(take);

            return await query.Select(t => TrackListingDTO.FromEntity(t)).ToListAsync();
        }

        private Dictionary<(FilterLabelType, FilterOperator), Expression<Func<Track, bool>>> GenerateFilters(ICollection<TrackFilterCondition>? conditions)
        {
            var filters = new Dictionary<(FilterLabelType, FilterOperator), Expression<Func<Track, bool>>>();

            if (conditions != null)
            {
                foreach (var condition in conditions)
                {
                    switch (condition.FilterLabelType)
                    {
                        case FilterLabelType.Album when condition.FilterOperator == FilterOperator.Equals:
                            string? albumEquals = condition.Value as string;
                            filters[(condition.FilterLabelType, condition.FilterOperator)] = (t) => t.Album!.ToLower() == albumEquals!.ToLower();
                            break;
                        case FilterLabelType.Album when condition.FilterOperator == FilterOperator.Like:
                            string? albumLike = condition.Value as string;
                            filters[(condition.FilterLabelType, condition.FilterOperator)] = (t) => t.Album!.ToLower().Contains(albumLike!.ToLower());
                            break;
                        case FilterLabelType.Category when condition.FilterOperator == FilterOperator.Equals:
                            var categoryDTO = condition.Value as CategoryDTO;
                            int? categoryId = categoryDTO?.Id as int?;
                            filters[(condition.FilterLabelType, condition.FilterOperator)] = (t) => t.Categories.Any(tc => tc.Id == categoryId);
                            break;

                        case FilterLabelType.DateAdded when condition.FilterOperator == FilterOperator.Equals:
                            DateTime? dateAddedEquals = condition.Value as DateTime?;
                            filters[(condition.FilterLabelType, condition.FilterOperator)] = (t) => t.DateAdded == dateAddedEquals;
                            break;
                        case FilterLabelType.DateAdded when condition.FilterOperator == FilterOperator.LessThan:
                            DateTime? dateAddedLessThan = condition.Value as DateTime?;
                            filters[(condition.FilterLabelType, condition.FilterOperator)] = (t) => t.DateAdded < dateAddedLessThan;
                            break;
                        case FilterLabelType.DateAdded when condition.FilterOperator == FilterOperator.GreaterThan:
                            DateTime? dateAddedGreaterThan = condition.Value as DateTime?;
                            filters[(condition.FilterLabelType, condition.FilterOperator)] = (t) => t.DateAdded > dateAddedGreaterThan;
                            break;

                        case FilterLabelType.DateModified when condition.FilterOperator == FilterOperator.Equals:
                            DateTime? dateModifiedEquals = condition.Value as DateTime?;
                            filters[(condition.FilterLabelType, condition.FilterOperator)] = (t) => t.DateModified == dateModifiedEquals;
                            break;
                        case FilterLabelType.DateModified when condition.FilterOperator == FilterOperator.LessThan:
                            DateTime? dateModifiedLessThan = condition.Value as DateTime?;
                            filters[(condition.FilterLabelType, condition.FilterOperator)] = (t) => t.DateModified < dateModifiedLessThan;
                            break;
                        case FilterLabelType.DateModified when condition.FilterOperator == FilterOperator.GreaterThan:
                            DateTime? dateModifiedGreaterThan = condition.Value as DateTime?;
                            filters[(condition.FilterLabelType, condition.FilterOperator)] = (t) => t.DateModified > dateModifiedGreaterThan;
                            break;

                        case FilterLabelType.ReleaseDate when condition.FilterOperator == FilterOperator.Equals:
                            DateTime? releaseDateEquals = condition.Value as DateTime?;
                            filters[(condition.FilterLabelType, condition.FilterOperator)] = (t) => t.ReleaseDate == releaseDateEquals;
                            break;
                        case FilterLabelType.ReleaseDate when condition.FilterOperator == FilterOperator.LessThan:
                            DateTime? releaseDateLessThan = condition.Value as DateTime?;
                            filters[(condition.FilterLabelType, condition.FilterOperator)] = (t) => t.ReleaseDate < releaseDateLessThan;
                            break;
                        case FilterLabelType.ReleaseDate when condition.FilterOperator == FilterOperator.GreaterThan:
                            DateTime? releaseDateGreaterThan = condition.Value as DateTime?;
                            filters[(condition.FilterLabelType, condition.FilterOperator)] = (t) => t.ReleaseDate > releaseDateGreaterThan;
                            break;

                        case FilterLabelType.Duration when condition.FilterOperator == FilterOperator.Equals:
                            var duration = (condition.Value as TimeSpan?);
                            double durationEquals = duration?.TotalSeconds ?? 0;
                            double durationUpper = durationEquals + 1;
                            filters[(condition.FilterLabelType, condition.FilterOperator)] = (t) => t.Duration >= durationEquals && t.Duration < durationUpper;
                            break;

                        case FilterLabelType.Duration when condition.FilterOperator == FilterOperator.LessThan:
                            double durationLessThan = (condition.Value as TimeSpan?)?.TotalSeconds ?? 0;
                            filters[(condition.FilterLabelType, condition.FilterOperator)] = (t) => t.Duration < durationLessThan;
                            break;

                        case FilterLabelType.Duration when condition.FilterOperator == FilterOperator.GreaterThan:
                            double durationGreaterThan = (condition.Value as TimeSpan?)?.TotalSeconds ?? 0;
                            filters[(condition.FilterLabelType, condition.FilterOperator)] = (t) => t.Duration > durationGreaterThan;
                            break;

                        case FilterLabelType.Status when condition.FilterOperator == FilterOperator.Equals:
                            TrackStatus? status = condition.Value as TrackStatus?;
                            filters[(condition.FilterLabelType, condition.FilterOperator)] = (t) => t.Status == status;
                            break;

                        case FilterLabelType.Type when condition.FilterOperator == FilterOperator.Equals:
                            TrackType? type = condition.Value as TrackType?;
                            filters[(condition.FilterLabelType, condition.FilterOperator)] = (t) => t.Type == type;
                            break;

                        case FilterLabelType.Title when condition.FilterOperator == FilterOperator.Equals:
                            var titleEquals = condition.Value as string;
                            filters[(condition.FilterLabelType, condition.FilterOperator)] = (t) => t.Title.ToLower() == titleEquals!.ToLower();
                            break;
                        case FilterLabelType.Title when condition.FilterOperator == FilterOperator.Like:
                            var titleLike = condition.Value as string;
                            filters[(condition.FilterLabelType, condition.FilterOperator)] = (t) => t.Title.ToLower().Contains(titleLike!.ToLower());
                            break;
                    }
                }
            }

            return filters;
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
            foreach (var t in tracks)
            {
                var categories = t.Categories.ToList();
                for (int i = 0; i < categories.Count; i++)
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
