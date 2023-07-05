using Microsoft.EntityFrameworkCore;
using RA.Database;
using RA.Database.Models.Enums;
using RA.DTO;

namespace RA.DAL
{
    public class TrackHistoryService : ITrackHistoryService
    {
        private readonly IDbContextFactory<AppDbContext> dbContextFactory;

        public TrackHistoryService(IDbContextFactory<AppDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task AddTrackToHistory(TrackHistoryDTO trackHistoryDTO)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var entity = TrackHistoryDTO.ToEntity(trackHistoryDTO);
            dbContext.TrackHistory.Add(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task<ICollection<TrackHistoryListingDTO>> RetrieveTrackHistory(DateTime dateMinPlayed)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var result = await dbContext.TrackHistory
                .Where(th => th.DatePlayed >= dateMinPlayed)
                .Include(th => th.Track)
                .Include(th => th.Track!.TrackArtists)
                .ThenInclude(ta => ta.Artist)
                .IgnoreQueryFilters()
                .OrderByDescending(th => th.DatePlayed)
                .Select(th => TrackHistoryListingDTO.FromEntity(th))
                .AsNoTracking()
                .ToListAsync();
            return result;
        }

        public async Task<ICollection<TrackHistoryListingDTO>> GetHistoryBetween(DateTime dateStart,
                                                                                 DateTime dateEnd,
                                                                                 IList<TrackType> trackTypes)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            if(dateStart == dateEnd)
            {
                dateEnd = dateStart.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            } else
            {
                dateEnd = dateEnd.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            }
            var result = await dbContext.TrackHistory
                .Where(th => th.DatePlayed >= dateStart && th.DatePlayed <= dateEnd)
                .Where(th => trackTypes.Contains(th.TrackType))
                .Include(th => th.Track)
                .IgnoreQueryFilters()
                .Include(th => th.Track!.TrackArtists)
                .ThenInclude(ta => ta.Artist)
                .OrderBy(th => th.DatePlayed)
                .Select(th => TrackHistoryListingDTO.FromEntity(th))
                .ToListAsync();
            return result;
        }

        public async Task<TrackHistoryListingDTO?> RetrieveItem(DateTime datePlayed)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var result = await dbContext.TrackHistory
                .Where(th => th.DatePlayed == datePlayed)
                .Include(th => th.Track)
                .Include(th => th.Track!.TrackArtists)
                .ThenInclude(ta => ta.Artist)
                .OrderByDescending(th => th.DatePlayed)
                .Select(th => TrackHistoryListingDTO.FromEntity(th))
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task GetMostPlayedTracks(DateTime dateStart,
                                              DateTime dateEnd,
                                              int max = 10,
                                              IList<TrackType>? trackTypesToExclude = null)
        {
            //TODO
            throw new NotImplementedException();
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var query = dbContext.TrackHistory
                .Include(th => th.Track)
                .Include(th => th.Track!.TrackArtists)
                .ThenInclude(ta => ta.Artist)
                .Where(th => th.DatePlayed >= dateStart && th.DatePlayed <= dateEnd);

            if (trackTypesToExclude != null && trackTypesToExclude.Any())
            {
                query = query.Where(th => !trackTypesToExclude.Contains(th.Track.Type));
            }

            var result = await query
                .GroupBy(th => new { th.TrackId, th.Track })
                .OrderByDescending(group => group.Count())
                .Take(max)
                .ToListAsync();

        }


    }
}
