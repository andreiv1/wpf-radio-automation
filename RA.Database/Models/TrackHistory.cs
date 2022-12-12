using RA.Database.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace RA.Database.Models
{
    public class TrackHistory
    {
        [Key]
        public DateTime DatePlayed { get; set; }
        public TrackType TrackType { get; set; }
        public Track? Track { get; set; }
        public int? TrackId { get; set; }
    }
}
