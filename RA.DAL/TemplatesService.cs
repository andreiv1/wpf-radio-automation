using Microsoft.EntityFrameworkCore;
using RA.Database;
using RA.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DAL
{
    public class TemplatesService : ITemplatesService
    {
        private readonly IDbContextFactory<AppDbContext> dbContextFactory;
        public TemplatesService(IDbContextFactory<AppDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }
        public async Task<IEnumerable<TemplateDto>> GetTemplatesAsync()
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            return await dbContext.Templates
                .Select(t => TemplateDto.FromEntity(t))
                .ToListAsync();
        }

        public async Task<IEnumerable<TemplateClockDto>> GetTemplatesForClockWithId(int clockId)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                return await dbContext.Templates
                      .Include(t => t.TemplateClocks)
                      .ThenInclude(tc => tc.Clock)
                      .Where(t => t.Id == clockId)
                      .SelectMany(t => t.TemplateClocks)
                      .Select(t => TemplateClockDto.FromEntity(t))
                      .ToListAsync();
            }

        }
    }
}
