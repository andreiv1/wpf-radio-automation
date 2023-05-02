using RA.Database.Models.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Database.Models
{
    [Table("Templates")]
    public partial class Template : BaseModel
    {
        [Required]
        public String Name { get; set; }
        public ICollection<ScheduleDefaultItem> ScheduleDefaultItems { get; set; }
        public ICollection<SchedulePlanned> PlannedSchedules { get; set; }
        public ICollection<ClockTemplate> TemplateClocks { get; set; }
    }
}
