using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.Core.Models
{
    public class DateRange
    {
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }

        public override string? ToString()
        {
            return $"{Start.ToString("dd/MM/yyyy")} - {End?.ToString("dd/MM/yyyy")}";
        }
    }
}
