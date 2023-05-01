﻿using RA.DTO;

namespace RA.DAL
{
    public interface ITagsService
    {
        Task<IEnumerable<TagCategoryDto>> GetTagCategoriesAsync();
        IEnumerable<TagCategoryDto> GetTagCategories();
        Task<IEnumerable<TagValueDto>> GetTagValuesByCategoryAsync(int tagCategoryId);
    }
}