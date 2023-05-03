using RA.Database.Models.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Database.Models
{
    public class TagCategory : BaseModel
    {
        [MaxLength(100)]
        public String Name { get; set; }
        public ICollection<TagValue> Values { get; set; }
    }
}
