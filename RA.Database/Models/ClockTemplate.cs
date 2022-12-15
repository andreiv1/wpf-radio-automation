using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Database.Models
{
    [Table("Clocks_Templates")]
    public class ClockTemplate
    {
        public int ClockId { get; set; }
        public virtual Clock Clock { get; set; }
        public int TemplateId { get; set; }
        public virtual Template Template { get; set; }
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// How many consecutive hours a clock should last
        /// </summary>
        [MaxLength(2)]
        public int ClockSpan { get; set; }
    }
}
