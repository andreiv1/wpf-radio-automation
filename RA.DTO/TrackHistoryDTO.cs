using RA.Database.Models;
using RA.Database.Models.Enums;

namespace RA.DTO
{
    public class TrackHistoryDTO
    {
        public TrackType TrackType { get; set; }
        public DateTime DatePlayed { get; set; }
        public int? TrackId { get; set; }
        public static TrackHistory ToEntity(TrackHistoryDTO dto)
        {
            return new TrackHistory
            {
                TrackType = dto.TrackType,
                DatePlayed = dto.DatePlayed,
                TrackId = dto.TrackId,
            };
        }
    }
}
