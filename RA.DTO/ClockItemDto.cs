using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Dto
{
    public class ClockItemDto
    {
        public int Id { get; set; }
        public int? TrackId { get; set; }
        public int? CategoryId { get; set; }
        public String CategoryName { get; set; }
        public int? ClockId { get; set; }

        public int OrderIndex { get; set; }

        public static ClockItemDto FromEntity(ClockItem clockItem)
        {
            return new ClockItemDto
            {
                Id = clockItem.Id,
                TrackId = clockItem.TrackId,
                CategoryId = clockItem.CategoryId,
                CategoryName = clockItem.Category?.Name ?? String.Empty,
                ClockId = clockItem.ClockId,
                OrderIndex = clockItem.OrderIndex,
            };
        }

        public static ClockItem ToEntity(ClockItemDto clockItemDto)
        {
            return new ClockItem
            {
                TrackId = clockItemDto.TrackId,
                CategoryId = clockItemDto.CategoryId,
                ClockId = clockItemDto?.ClockId,
                OrderIndex = clockItemDto.OrderIndex
            };
        }
    }
}
