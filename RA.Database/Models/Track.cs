using RA.Database.Models.Abstract;
using RA.Database.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RA.Database.Models
{
    [Table("Tracks")]
    public partial class Track : BaseModel
    {
        [Required]
        public TrackType Type { get; set; }

        [Required]
        public TrackStatus Status { get; set; }

        [Required]
        public String Title { get; set; }

        public double Duration { get; set; }
        public DateTime? ReleaseDate { get; set; }

        public String Album { get; set; }
        public String Comments { get; set; }

        public String Lyrics { get; set; }

        [Required]
        public String FilePath { get; set; }
        public String ImageName { get; set; }
        public int? Bpm { get; set; }
        public String ISRC { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public DateTime? DateModified { get; set; }

        public DateTime? DateDeleted { get; set; }

        public ICollection<Category> Categories { get; set; }
        public ICollection<ArtistTrack> TrackArtists { get; set; }
        public ICollection<TrackTag> TrackTags { get; set; }

    }
}