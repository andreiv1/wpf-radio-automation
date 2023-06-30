using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Database.Models
{
    [Table("ClockItemsCategory_Tags")]
    public class ClockItemCategoryTag
    {
        public int ClockItemCategoryId { get; set; }
        public ClockItemCategory ClockItemCategory { get; set; }
        public int TagValueId { get; set; }
        public TagValue TagValue { get; set; }
    }
}
