using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Database.Models
{
    public class DefaultSchedule : Schedule
    {
        public DayOfWeek DayOfWeek { get; set; }
    }
}
