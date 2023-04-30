using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DTO
{
    public class ClockTemplateDto
    {
        public int ClockId { get; set; }
        public int TemplateId { get; set; }
        public string TemplateName { get; set; }
        public TimeSpan StartTime { get; set; } 
        public int ClockSpan { get; set; }

        public static ClockTemplateDto FromEntity(ClockTemplate entity)
        {
            return new ClockTemplateDto
            {
                ClockId = entity.ClockId,
                TemplateId = entity.TemplateId,
                //TemplateName = entity.Template.Name,
                StartTime = entity.StartTime,
                ClockSpan = entity.ClockSpan,
            };
        }
    }
}
