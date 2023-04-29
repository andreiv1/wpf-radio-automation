using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Dto.Abstract
{
    public abstract class ScheduleDto
    {
        public int? Id { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public TemplateDto? TemplateDto { get; set; }
    }
}
