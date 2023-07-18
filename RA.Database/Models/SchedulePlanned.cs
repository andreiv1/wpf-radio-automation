using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RA.Database.Models.Abstract;
using RA.Database.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }
        [EnumDataType(typeof(SchedulePlannedType))]
        public SchedulePlannedType Type { get; set; }

        [EnumDataType(typeof(SchedulePlannedFrequency))]
        public SchedulePlannedFrequency? Frequency { get; set; }

        public int TemplateId { get; set; }
        public Template Template { get; set; }
        public Boolean? IsMonday { get; set; }
        public Boolean? IsTuesday { get; set; }
        public Boolean? IsWednesday { get; set; }
        public Boolean? IsThursday { get; set; }
        public Boolean? IsFriday { get; set; }
        public Boolean? IsSaturday { get; set; }
        public Boolean? IsSunday { get; set; } 
    }
}
