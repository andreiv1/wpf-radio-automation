using RA.Database.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Database.Models
{
    public class UserGroup : BaseModel
    {
        public String Name { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<UserRule> Rules { get; set; }
    }
}
