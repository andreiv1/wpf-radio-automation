using RA.Database.Models;
using RA.DTO.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DTO
{
    public class ClockItemCategoryDTO : ClockItemBaseDTO
    {
        public int? CategoryId { get; set; }
        public String? CategoryName { get; private set; }
        public String? CategoryColor { get; private set; }
        public int? ArtistSeparation { get; set; }
        public int? TitleSeparation { get; set; }
        public int? TrackSeparation { get; set; }

        public static ClockItemCategoryDTO FromEntity(ClockItemCategory entity)
        {
            return new ClockItemCategoryDTO
            {
                Id = entity.Id,
                OrderIndex = entity.OrderIndex,
                ClockId = entity.ClockId,
                CategoryId = entity.CategoryId,
                CategoryName = entity.Category?.Name,
                CategoryColor = entity.Category?.Color,
                ArtistSeparation = entity.ArtistSeparation,
                TitleSeparation = entity.TitleSeparation,
                TrackSeparation = entity.TrackSeparation
            };
        }

        public static ClockItemCategory ToEntity(ClockItemCategoryDTO dto)
        {
            return new ClockItemCategory
            {
                Id = dto.Id,
                OrderIndex = dto.OrderIndex,
                ClockId = dto.ClockId,
                CategoryId = dto.CategoryId,
                ArtistSeparation= dto.ArtistSeparation,
                TitleSeparation= dto.TitleSeparation,
                TrackSeparation= dto.TrackSeparation
            };
        }
    }
}
