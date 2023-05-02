using RA.Database.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Database.Models
{
    public enum SchedulePlannedType
    {
        OneTime = 0,
        Recurrent = 1,
    }

    public enum SchedulePlannedFrequency
    {
        EveryWeek = 1,
        EveryTwoWeeks = 2,
    }
    public class SchedulePlanned : ScheduleBase
    {
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public SchedulePlannedType Type { get; set; }
        public SchedulePlannedFrequency? Frequency { get; set; }
    }
}
