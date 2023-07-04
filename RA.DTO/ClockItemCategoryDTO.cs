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
        public TimeSpan? MinDuration { get; set; }
        public TimeSpan? MaxDuration { get; set; }
        public DateTime? MinReleaseDate { get; set; }
        public DateTime? MaxReleaseDate { get; set; }
        public bool IsFiller { get; set; }
        public List<ClockItemCategoryTagDTO>? Tags { get; set; }
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
                MinDuration = entity.MinDuration,
                MaxDuration = entity.MaxDuration,
                MinReleaseDate = entity.MinReleaseDate,
                MaxReleaseDate = entity.MaxReleaseDate,
                IsFiller = entity.IsFiller,

                ClockItemEventId = entity.ClockItemEventId,
                EventOrderIndex = entity.EventOrderIndex,

                Tags = entity.ClockItemCategoryTags != null ?
                    entity.ClockItemCategoryTags.Select(ct => ClockItemCategoryTagDTO.FromEntity(ct)).ToList() : null,
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
                ArtistSeparation = dto.ArtistSeparation,
                TitleSeparation = dto.TitleSeparation,
                TrackSeparation = dto.TrackSeparation,

                MinDuration = dto.MinDuration,
                MaxDuration = dto.MaxDuration,
                MinReleaseDate = dto.MinReleaseDate,
                MaxReleaseDate = dto.MaxReleaseDate,
                IsFiller = dto.IsFiller,

                ClockItemCategoryTags = dto.Tags != null ?
                    dto.Tags.Select(t => ClockItemCategoryTagDTO.ToEntity(t)).ToList() : new(),

            };
        }
    }
}
