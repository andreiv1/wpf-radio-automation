using RA.Database.Models.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Database.Models
{
    public class User : BaseModel
    {
        [Required]
        [MaxLength(100)]
        public String Username { get; set; }

        [Required]
        [MaxLength(100)]
        public String Password { get; set; }

        [Required]
        [MaxLength(50)]
        public String FullName { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateModified { get; set; }
        public ICollection<Playlist> Playlists { get; set; }
    }
}
