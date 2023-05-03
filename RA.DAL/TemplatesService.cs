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

        public async Task AddTemplate(TemplateDto templateDto)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var entity = TemplateDto.ToEntity(templateDto);
            await dbContext.Templates.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateTemplate(TemplateDto templateDto)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var entity = TemplateDto.ToEntity(templateDto);
            dbContext.Templates.Update(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task<TemplateDto> GetTemplate(int templateId)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var dto = await dbContext.Templates.Where(t => t.Id == templateId)
                .Select(t => TemplateDto.FromEntity(t))
                .FirstOrDefaultAsync();
            if(dto == null)
            {
                throw new Exception($"Template with id {templateId} not found");
            }
            return dto;

        }
    }
}
