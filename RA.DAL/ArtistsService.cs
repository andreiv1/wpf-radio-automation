using Microsoft.EntityFrameworkCore;
using RA.Database;
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

        public async Task<int> GetArtistsCountAsync()
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            return await dbContext.Artists.CountAsync();
        }

        public async Task<IEnumerable<ArtistDTO>> GetArtistsAsync(int skip, int take)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                return await dbContext.Artists
                    .Skip(skip)
                    .Take(take)
                    .Select(a => ArtistDTO.FromEntity(a))
                    .ToListAsync();
            }
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
