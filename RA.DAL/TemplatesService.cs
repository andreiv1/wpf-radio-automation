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
        public async Task<IEnumerable<TemplateDTO>> GetTemplatesAsync()
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            return await dbContext.Templates
                .Select(t => TemplateDTO.FromEntity(t))
                .ToListAsync();
        }

        public async Task<IEnumerable<TemplateClockDTO>> GetTemplatesForClockWithId(int clockId)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                return await dbContext.Templates
                      .Include(t => t.TemplateClocks)
                      .ThenInclude(tc => tc.Clock)
                      .Where(t => t.Id == clockId)
                      .SelectMany(t => t.TemplateClocks)
                      .Select(t => TemplateClockDTO.FromEntity(t))
                      .ToListAsync();
            }

        }

        public async Task AddTemplate(TemplateDTO templateDto)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var entity = TemplateDTO.ToEntity(templateDto);
            await dbContext.Templates.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateTemplate(TemplateDTO templateDto)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var entity = TemplateDTO.ToEntity(templateDto);
            dbContext.Templates.Update(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task<TemplateDTO> GetTemplate(int templateId)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var dto = await dbContext.Templates.Where(t => t.Id == templateId)
                .Select(t => TemplateDTO.FromEntity(t))
                .FirstOrDefaultAsync();
            if(dto == null)
            {
                throw new Exception($"Template with id {templateId} not found");
            }
            return dto;

        }

        public async Task AddClockToTemplate(ClockTemplateDTO clockTemplate)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var entity = ClockTemplateDTO.ToEntity(clockTemplate);
            dbContext.ClockTemplates.Add(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateClockInTemplate(ClockTemplateDTO clockTemplate)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var entity = ClockTemplateDTO.ToEntity(clockTemplate);
            dbContext.ClockTemplates.Update(entity);
            await dbContext.SaveChangesAsync();
        }
    }
}
