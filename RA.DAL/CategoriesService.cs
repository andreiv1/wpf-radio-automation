﻿using Microsoft.EntityFrameworkCore;
using RA.DAL.Exceptions;
using RA.Database;
using RA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DAL
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IDbContextFactory<AppDbContext> dbContextFactory;

        public CategoriesService(IDbContextFactory<AppDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task<IEnumerable<CategoryDTO>> GetRootCategoriesAsync()
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            return await dbContext.GetRootCategories()
                .Select(c => CategoryDTO.FromEntity(c))
                .ToListAsync();
        }

        public async Task<bool> HasCategoryChildren(int categoryId)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            return await dbContext.Categories
                .Where(c => c.ParentId == categoryId)
                .AnyAsync();
        }

        public async Task<IEnumerable<CategoryDTO>> GetChildrenCategoriesAsync(int parentCategoryId)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            return await dbContext.Categories
                .Where(c => c.ParentId == parentCategoryId)
                .Select(c => CategoryDTO.FromEntity(c))
                .ToListAsync();
        }


        public async Task<CategoryHierarchyDTO> GetCategoryHierarchy(int categoryId)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var result = await dbContext.CategoriesHierarchy
                        .Where(ch => ch.Id == categoryId)
                        .Select(ch => CategoryHierarchyDTO.FromEntity(ch))
                        .FirstOrDefaultAsync();
            if(result == null)
            {
                throw new NotFoundException($"Category with id {categoryId} not found");
            }

            return result;
        }

    }
}
