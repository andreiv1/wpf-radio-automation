using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Database.Models
{
    public enum PlannedScheduleType
    {
        OneTime,
        Recurrent
    }

    public enum PlannedScheduleFrequency
    {
        EveryWeek,
        EveryTwoWeeks
    }
    public class PlannedSchedule : Schedule
    {
        public PlannedScheduleType Type { get; set; }

        public PlannedScheduleFrequency? Frequency { get; set; }

    }
}
