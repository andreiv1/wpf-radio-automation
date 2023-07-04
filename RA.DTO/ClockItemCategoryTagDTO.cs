using RA.Database.Models;

namespace RA.DTO
{
    public class ClockItemCategoryTagDTO
    {
        public int ClockItemId { get; set; }
        public int TagValueId { get; set; }
        public int TagCategoryId { get; private set; }

        public static ClockItemCategoryTagDTO FromEntity(ClockItemCategoryTag entity)
        {
            return new ClockItemCategoryTagDTO
            {
                ClockItemId = entity.ClockItemCategoryId,
                TagValueId = entity.TagValueId,
                TagCategoryId = entity.TagValue.TagCategoryId,
            };
        }

        public static ClockItemCategoryTag ToEntity(ClockItemCategoryTagDTO dto)
        {
            return new ClockItemCategoryTag
            {
                ClockItemCategoryId = dto.ClockItemId,
                TagValueId = dto.TagValueId,
            };
        }
    }
}
