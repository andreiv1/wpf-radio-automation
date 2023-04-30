using RA.DTO.Abstract;
using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DTO
{
    public class DefaultScheduleDto : ScheduleDto
    {
        public DayOfWeek Day { get; set; }

        public static DefaultScheduleDto FromEntity(DefaultSchedule entity)
        {
            return new DefaultScheduleDto
            {
                Id = entity.Id,
                Day = entity.DayOfWeek,
                TemplateDto = new TemplateDto { Id = entity.TemplateId, Name = entity.Template.Name },
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
            };
        }

        public static DefaultSchedule ToEntity(DefaultScheduleDto dto)
        {
            return new DefaultSchedule
            {
                Id = dto.Id.GetValueOrDefault(),
                DayOfWeek = dto.Day,
                StartDate = dto.StartDate.GetValueOrDefault(),
                EndDate = dto.EndDate.GetValueOrDefault(),
                TemplateId = dto.TemplateDto.Id,
            };
        }
    }
}
