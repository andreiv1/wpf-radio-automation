﻿using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<TagValueDTO>> GetTagValuesByCategoryNameAsync(string name)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            return await dbContext.TagValues
                .Include(tv => tv.TagCategory)
                .Where(tv => tv.TagCategory.Name == name)
                .Select(tv => TagValueDTO.FromEntity(tv))
                .ToListAsync();
        }

        public async Task<TagValueDTO?> AddTagValue(string tagCategory, string value)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var tagCategoryEntity = await dbContext.TagCategories
                .FirstOrDefaultAsync(tc => tc.Name == tagCategory);
            if (tagCategoryEntity == null)
            {
               tagCategoryEntity = new TagCategory
               {
                    Name = tagCategory
               };
               dbContext.TagCategories.Add(tagCategoryEntity);
               await dbContext.SaveChangesAsync();
            }


            var tagValueEntity = await dbContext.TagValues
                .FirstOrDefaultAsync(tv => tv.Name == value && tv.TagCategoryId == tagCategoryEntity.Id);
            if(tagValueEntity == null)
            {
                tagValueEntity = new TagValue
                {
                    TagCategoryId = tagCategoryEntity.Id,
                    Name = value
                };
                dbContext.TagValues.Add(tagValueEntity);
                await dbContext.SaveChangesAsync();
            }
            
            return TagValueDTO.FromEntity(tagValueEntity);
        }
    }
}
