using Microsoft.EntityFrameworkCore;
using RA.Database;
using RA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Logic.Planning
{
    public class PlaylistGenerator : IPlaylistGenerator
    {
        private readonly IDbContextFactory<AppDbContext> dbContextFactory;

        public PlaylistGenerator(IDbContextFactory<AppDbContext> dbContextFactory) {
            this.dbContextFactory = dbContextFactory;
        }
        public PlaylistDTO GeneratePlaylistForDate(DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}
