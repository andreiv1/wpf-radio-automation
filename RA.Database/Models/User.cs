using RA.Database.Models.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Database.Models
{
    [Table("Users")]
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
        public UserGroup UserGroup { get; set; }

        [Required]
        public int UserGroupId { get; set; }
    }
}
