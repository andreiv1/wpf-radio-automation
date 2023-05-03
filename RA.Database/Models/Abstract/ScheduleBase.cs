using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Database.Models.Abstract
{
    public abstract class ScheduleBase : BaseModel
    {
        [MaxLength(300)]
        public String Name { get; set; }
    }
}
