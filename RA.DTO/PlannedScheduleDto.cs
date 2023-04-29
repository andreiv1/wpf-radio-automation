using RA.Dto.Abstract;
using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Dto
{
    public class PlannedScheduleDto : ScheduleDto
    {
        public PlannedScheduleType Type { get; set; }
        public PlannedScheduleFrequency? Frequency { get; set; }

        public static PlannedScheduleDto FromEntity(PlannedSchedule entity)
        {
            return new PlannedScheduleDto
            {
                Id = entity.Id,
                Type = entity.Type,
                Frequency = entity.Frequency,
                TemplateDto = TemplateDto.FromEntity(entity.Template),
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
            };
        }
    }
}
