using RA.Database.Models.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace RA.Database.Models
{
    [Table("PlaylistItems")]
    public class PlaylistItem : BaseModel
    {
        public DateTime ETA { get; set; }

        [Column(TypeName = "double(11,5)")]
        public double Length { get; set; }
        public Track Track { get; set; }
        public string Label { get; set; }
        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; }
        public virtual PlaylistItem ParentPlaylistItem { get; set; }
        public int? ParentPlaylistItemId { get; set; }
    }
}
