using RA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DAL
{
    public interface ICategoriesService
    {
        Task<CategoryHierarchyDto> GetCategoryHierarchy(int categoryId);
        Task<IEnumerable<CategoryDto>> GetChildrenCategoriesAsync(int parentCategoryId);
        Task<IEnumerable<CategoryDto>> GetRootCategoriesAsync();
        Task<bool> HasCategoryChildren(int categoryId);
    }
}
