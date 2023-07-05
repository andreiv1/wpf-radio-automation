using Microsoft.EntityFrameworkCore;
using RA.Database;
using RA.Database.Models;
using RA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DAL
{
    public class ArtistsService : IArtistsService
    {
        private readonly IDbContextFactory<AppDbContext> dbContextFactory;
        public ArtistsService(IDbContextFactory<AppDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        //This can be moved to RA.Database.Queries
        private IQueryable<Artist> GetArtistQuery(AppDbContext dbContext, int skip, int take, string query)
        {
            IQueryable<Artist> result;
            query = query.Trim();
            if (string.IsNullOrEmpty(query))
            {
                result = dbContext.Artists
                      .Skip(skip)
                      .Take(take);
            }
            else
            {
                result = dbContext.Artists
                     .Skip(skip)
                     .Take(take)
                     .Where(a => a.Name.ToLower().Contains(query));
            }
            return result;
        }
        public async Task<int> GetArtistsCountAsync(int skip, int take, string query = "")
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var result = GetArtistQuery(dbContext, skip, take, query);
            return await result.CountAsync();
        }

        public async Task<IEnumerable<ArtistDTO>> GetArtistsAsync(int skip, int take, string query = "")
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var result = GetArtistQuery(dbContext, skip, take, query);
            return await result.AsNoTracking()
                  .Select(a => ArtistDTO.FromEntity(a))
                  .ToListAsync();

        }

        public async Task<ArtistDTO?> GetArtistByName(string name)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            return await dbContext.Artists
                .Where(a => a.Name.Equals(name))
                .Select(a => ArtistDTO.FromEntity(a))
                .FirstOrDefaultAsync();
        }

        public async Task AddArtist(ArtistDTO artist)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var entity = ArtistDTO.ToEntity(artist);
            await dbContext.Artists.AddAsync(entity);
            await dbContext.SaveChangesAsync();

        }
    }
}
