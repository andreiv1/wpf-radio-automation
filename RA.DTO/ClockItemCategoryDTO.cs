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

        public int? MinBpm { get; set; }
        public int? MaxBpm { get; set; }
        public TimeSpan? MinDuration { get; set; }
        public TimeSpan? MaxDuration { get; set; }
        public DateTime? MinReleaseDate { get; set; }
        public DateTime? MaxReleaseDate { get; set; }
        public Boolean IsFiller { get; set; } = false;

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
                TrackSeparation = entity.TrackSeparation,
                MinBpm = entity.MinBpm,
                MaxBpm = entity.MaxBpm,
                MinDuration = entity.MinDuration,
                MaxDuration = entity.MaxDuration,
                MinReleaseDate = entity.MinReleaseDate,
                MaxReleaseDate = entity.MaxReleaseDate,
                IsFiller = entity.IsFiller,
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
                TrackSeparation= dto.TrackSeparation,
                MinBpm= dto.MinBpm,
                MaxBpm= dto.MaxBpm,
                MinDuration = dto.MinDuration,
                MaxDuration = dto.MaxDuration,
                MinReleaseDate = dto.MinReleaseDate,
                MaxReleaseDate = dto.MaxReleaseDate,
                IsFiller = dto.IsFiller,

            };
        }
    }
}
