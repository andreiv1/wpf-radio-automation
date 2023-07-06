using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Database.Models.Abstract
{
    [Table("ClockItems")]
    public abstract class ClockItemBase : BaseModel
    {
        public int OrderIndex { get; set; }
        public int ClockId { get; set; }
        public Clock Clock { get; set; }
        public int? ClockItemEventId { get; set; }
        public ClockItemEvent? ClockItemEvent { get; set; }
        public int? EventOrderIndex { get; set; }
    }
}
