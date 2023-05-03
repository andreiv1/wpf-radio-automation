using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DTO
{
    public class ScheduleDefaultItemDTO
    {
        public int? Id { get; set; }
        public  ScheduleDefaultDTO Schedule { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TemplateDTO? Template { get; set; }

        public ScheduleDefaultItemDTO(ScheduleDefaultDTO schedule)
        {
            Schedule = schedule;
        }

        public static ScheduleDefaultItemDTO FromEntity(ScheduleDefaultItem entity, ScheduleDefaultDTO parentDefaultScheduleDto)
        {
            return new ScheduleDefaultItemDTO(parentDefaultScheduleDto)
            {
                Id = entity.Id,
                DayOfWeek = entity.DayOfWeek,
                Template = TemplateDTO.FromEntity(entity.Template)
            };
        }

        public static ScheduleDefaultItem ToEntity(ScheduleDefaultItemDTO dto)
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
                Template = TemplateDTO.ToEntity(dto.Template),
            };
        }
            
    }
}
