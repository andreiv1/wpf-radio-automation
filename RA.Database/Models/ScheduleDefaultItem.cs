using RA.Database.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Database.Models
{
    public class ScheduleDefaultItem : BaseModel
    {
        public ScheduleDefault Schedule { get; set; }
        public int ScheduleId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public int TemplateId { get; set; }
        public Template Template { get; set; }
    }
}
