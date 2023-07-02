using RA.Database.Models;
using RA.Database.Models.Enums;

namespace RA.DTO
{
    public class TrackHistoryDTO
    {
        public TrackType TrackType { get; set; }
        public DateTime DatePlayed { get; set; }
        public TimeSpan LengthPlayed { get; set; }
        public int? TrackId { get; set; }
        public static TrackHistory ToEntity(TrackHistoryDTO dto)
        {
            return new TrackHistory
            {
                TrackType = dto.TrackType,
                DatePlayed = dto.DatePlayed,
                LengthPlayed = dto.LengthPlayed,
                TrackId = dto.TrackId,
            };
        }
    }
}
