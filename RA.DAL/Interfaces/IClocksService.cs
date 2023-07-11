using RA.DTO;
using RA.DTO.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DAL
{
    public interface IClocksService
    {
        IEnumerable<ClockDTO> GetClocks();
        Task<IEnumerable<ClockDTO>> GetClocksAsync();
        IEnumerable<ClockItemBaseDTO> GetClockItems(int clockId);
        Task<IEnumerable<ClockItemBaseDTO>> GetClockItemsAsync(int clockId);
        Task<Dictionary<int, TimeSpan>> CalculateAverageDurationsForCategoriesInClockWithId(int clockId);
        Task AddClock(ClockDTO clockDto);
        Task<ClockDTO> GetClock(int id);
        Task UpdateClock(ClockDTO clockDto);
        Task AddClockItem(ClockItemBaseDTO clockItemDto);
        Task<ClockItemBaseDTO> GetClockItemAsync(int clockItemId);
        Task DeleteClockItem(int clockItemId);

        Task<bool> RemoveClock(int clockId);
        Task UpdateClockItem(ClockItemCategoryDTO clockItemCategoryDto);
        Task DuplicateClockItems(ICollection<int> clockItemsIds, int clockId);
    }
}
