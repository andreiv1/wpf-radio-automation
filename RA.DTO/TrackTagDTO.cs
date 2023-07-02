using RA.Database.Models;

namespace RA.DTO
{
    public class TrackTagDTO
    {
        public int TrackId { get; set; }
        public int TagValueId { get; set; }
        public int TagCategoryId { get; set; } 

        public static TrackTagDTO FromEntity(TrackTag entity)
        {
            return new TrackTagDTO
            {
                TagValueId = entity.TagValueId,
                TrackId = entity.TrackId,
                TagCategoryId = entity.TagValue.TagCategoryId
            };
        }

        public static TrackTag ToEntity(TrackTagDTO dto)
        {
            return new TrackTag
            {
                TagValueId = dto.TagValueId,
                TrackId = dto.TrackId
            };
        }
    }
}
