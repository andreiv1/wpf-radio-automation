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
        Task<CategoryHierarchyDTO> GetCategoryHierarchy(int categoryId);
        Task<IEnumerable<CategoryDTO>> GetChildrenCategoriesAsync(int parentCategoryId);
        Task<IEnumerable<CategoryDTO>> GetRootCategoriesAsync();
        Task<bool> HasCategoryChildren(int categoryId);
    }
}
