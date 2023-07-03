using Microsoft.EntityFrameworkCore;
using RA.Database;
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
            ICollection<TrackHistoryListingDTO> history = new List<TrackHistoryListingDTO>();
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var result = await dbContext.TrackHistory
                .Where(th => th.DatePlayed >= dateMinPlayed)
                .Include(th => th.Track)
                .Include(th => th.Track!.TrackArtists)
                .ThenInclude(ta => ta.Artist)
                .Select(th => TrackHistoryListingDTO.FromEntity(th))
                .ToListAsync();
            return result;
        }


    }
}
