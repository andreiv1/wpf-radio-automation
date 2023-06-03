using Microsoft.EntityFrameworkCore;
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
        [EnumDataType(typeof(TrackType))]
        public TrackType Type { get; set; }

        [Required]
        [EnumDataType(typeof(TrackStatus))]
        public TrackStatus Status { get; set; }

        [Required]
        [MaxLength(200)]
        public String Title { get; set; }

        [Column(TypeName= "double(11,5)")]
        public double Duration { get; set; }

        [Column(TypeName = "double(11,5)")]
        public double? StartCue { get; set; }

        [Column(TypeName = "double(11,5)")]
        public double? NextCue { get; set; }

        [Column(TypeName = "double(11,5)")]
        public double? EndCue { get; set; }

        public DateTime? ReleaseDate { get; set; }

        [MaxLength(200)]
        public String Album { get; set; }
        public String Comments { get; set; }
        public String Lyrics { get; set; }

        [MaxLength(500)]
        public String FilePath { get; set; }

        [MaxLength(50)]
        public String ImageName { get; set; }
        public int? Bpm { get; set; }

        [MaxLength(55)]
        public String ISRC { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public DateTime? DateModified { get; set; }

        public DateTime? DateDeleted { get; set; }

        public ICollection<Category> Categories { get; set; }
        public ICollection<ArtistTrack> TrackArtists { get; set; }
        public ICollection<TrackTag> TrackTags { get; set; }

    }
}