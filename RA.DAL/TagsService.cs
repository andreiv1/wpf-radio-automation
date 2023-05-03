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
    public class TagsService : ITagsService
    {
        private readonly IDbContextFactory<AppDbContext> dbContextFactory;

        public TagsService(IDbContextFactory<AppDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public IEnumerable<TagCategoryDTO> GetTagCategories()
        {
            return GetTagCategoriesAsync().Result;
        }

        public async Task<IEnumerable<TagCategoryDTO>> GetTagCategoriesAsync()
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            return await dbContext.TagCategories
                .Select(tc => TagCategoryDTO.FromEntity(tc))
                .ToListAsync();
        }

        
        public async Task<IEnumerable<TagValueDTO>> GetTagValuesByCategoryAsync(int tagCategoryId)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            return await dbContext.TagValues
                .Where(tv => tv.TagCategoryId == tagCategoryId)
                .Select(tv => TagValueDTO.FromEntity(tv))
                .ToListAsync();
        }
    }
}
