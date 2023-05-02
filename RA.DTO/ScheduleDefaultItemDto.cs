using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DTO
{
    public class ScheduleDefaultItemDto
    {
        public int? Id { get; set; }
        public  ScheduleDefaultDto Schedule { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TemplateDto? Template { get; set; }

        public ScheduleDefaultItemDto(ScheduleDefaultDto schedule)
        {
            Schedule = schedule;
        }

        public static ScheduleDefaultItemDto FromEntity(ScheduleDefaultItem entity, ScheduleDefaultDto parentDefaultScheduleDto)
        {
            return new ScheduleDefaultItemDto(parentDefaultScheduleDto)
            {
                Id = entity.Id,
                DayOfWeek = entity.DayOfWeek,
                Template = TemplateDto.FromEntity(entity.Template)
            };
        }

        public static ScheduleDefaultItem ToEntity(ScheduleDefaultItemDto dto)
        {
            if(dto.Template == null)
            {
                throw new ArgumentException($"Item must have attached a template");
            }
            return new ScheduleDefaultItem
            {
                Id = dto.Id.GetValueOrDefault(),
                ScheduleId = dto.Schedule.Id.GetValueOrDefault(),
                DayOfWeek = dto.DayOfWeek,
                Template = TemplateDto.ToEntity(dto.Template),
            };
        }
            
    }
}
