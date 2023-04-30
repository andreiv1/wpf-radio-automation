using Microsoft.EntityFrameworkCore;
using RA.Database;
using RA.DTO;
using RA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DAL
{
    public class TagsService : ITagsService
    {
        private readonly IDbContextFactory<AppDbContext> dbContextFactory;

        public TagsService(IDbContextFactory<AppDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public IEnumerable<TagCategoryDto> GetTagCategories()
        {
            return GetTagCategoriesAsync().Result;
        }

        public async Task<IEnumerable<TagCategoryDto>> GetTagCategoriesAsync()
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            return await dbContext.TagCategories
                .Select(tc => TagCategoryDto.FromEntity(tc))
                .ToListAsync();
        }

        
        public async Task<IEnumerable<TagValueDto>> GetTagsByCategoryAsync(int tagCategoryId)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            return await dbContext.TagValues
                .Select(tv => TagValueDto.FromEntity(tv))
                .ToListAsync();
        }
    }
}
