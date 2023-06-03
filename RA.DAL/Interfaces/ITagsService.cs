using RA.DTO;

namespace RA.DAL
{
    public interface ITagsService
    {
        Task<IEnumerable<TagCategoryDTO>> GetTagCategoriesAsync();
        IEnumerable<TagCategoryDTO> GetTagCategories();
        Task<IEnumerable<TagValueDTO>> GetTagValuesByCategoryAsync(int tagCategoryId);
        Task<IEnumerable<TagValueDTO>> GetTagValuesByCategoryNameAsync(string name);
    }
}