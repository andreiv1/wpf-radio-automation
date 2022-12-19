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
    [Table("UserGroups")]
    public class UserGroup : BaseModel
    {
        [MaxLength(100)]
        public String Name { get; set; }
        public bool IsBuiltIn { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<UserGroupRule> Rules { get; set; }
    }
}
