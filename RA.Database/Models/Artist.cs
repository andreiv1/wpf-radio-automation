using RA.Database.Models.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RA.Database.Models
{
    [Table("Artists")]
    public class Artist : BaseModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(1500)]
        public string? Description { get; set; }

        public ICollection<ArtistTrack> ArtistTracks { get; set; }
    }
}
