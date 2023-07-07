using RA.Database.Models.Abstract;
using RA.Database.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RA.Database.Models
{
    [Table("PlaylistItems")]
    public class PlaylistItem : BaseModel
    {
        public DateTime ETA { get; set; }

        [Column(TypeName = "double(11,5)")]
        public double Length { get; set; }
        public Track? Track { get; set; }
        public EventType? EventType { get; set; }

        [MaxLength(400)]
        public string? Label { get; set; }
        public DateTime? NiceETA { get; set; }
        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; }
        public virtual PlaylistItem? ParentPlaylistItem { get; set; }
        public int? ParentPlaylistItemId { get; set; }
    }
}
