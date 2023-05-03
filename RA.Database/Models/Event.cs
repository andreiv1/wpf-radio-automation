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
    [Table("Events")]
    public class Event : BaseModel
    {
        [MaxLength(100)]
        public String Name { get; set; }

        
        [EnumDataType(typeof(EventType))]
        public EventType Type { get; set; }

        [MaxLength(100)]
        public String Command { get; set; }
        public ClockItem ClockItem { get; set; }
        public TimeSpan EstimatedDuration { get; set; }
    }
}
