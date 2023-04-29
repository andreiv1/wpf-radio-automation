using RA.Database.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Database.Models
{
    public class UserRule : BaseModel
    {
        public String RuleName { get; set; }
        public ICollection<UserGroup> Groups { get; set; } 
    }
}
