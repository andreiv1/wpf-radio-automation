using RA.DTO.Abstract;
using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DTO
{
    [Obsolete]
    public class PlannedScheduleDto : ScheduleBaseDto
    {
        public SchedulePlannedType Type { get; set; }
        public SchedulePlannedFrequency? Frequency { get; set; }

        //public static PlannedScheduleDto FromEntity(PlannedSchedule entity)
        //{
        //    return new PlannedScheduleDto
        //    {
        //        Id = entity.Id,
        //        Type = entity.Type,
        //        Frequency = entity.Frequency,
        //        TemplateDto = TemplateDto.FromEntity(entity.Template),
        //        StartDate = entity.StartDate,
        //        EndDate = entity.EndDate,
        //    };
        //}
    }
}
