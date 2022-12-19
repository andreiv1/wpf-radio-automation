using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RA.Database.Models.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Database.Models
{
    public class ScheduleDefault : ScheduleBase
    {
        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }
        public ICollection<ScheduleDefaultItem> ScheduleDefaultItems { get; set; }
    }
}
