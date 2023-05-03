using RA.DTO;
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
        IEnumerable<ClockItemDTO> GetClockItems(int clockId);
        Task<IEnumerable<ClockItemDTO>> GetClockItemsAsync(int clockId);
        Task<Dictionary<int, TimeSpan>> CalculateAverageDurationsForCategoriesInClockWithId(int clockId);
        Task AddClock(ClockDTO clockDto);
        Task<ClockDTO> GetClock(int id);
        Task UpdateClock(ClockDTO clockDto);
    }
}
