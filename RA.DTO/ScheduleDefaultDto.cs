using RA.DTO.Abstract;
using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DTO
{
    public class ScheduleDefaultDTO : ScheduleBaseDTO
    {
        public ICollection<ScheduleDefaultItemDTO>? Items { get; set; }

        public static ScheduleDefaultDTO FromEntity(ScheduleDefault entity)
        {
            var dto = new ScheduleDefaultDTO()
            {
                Id = entity.Id,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Name = entity.Name,

            };
            dto.Items = entity.ScheduleDefaultItems?
                    .Select(sd => ScheduleDefaultItemDTO.FromEntity(sd, dto))
                    .ToList();
            return dto;
        }

        public static ScheduleDefault ToEntity(ScheduleDefaultDTO scheduleDefaultDto)
        {
            return new ScheduleDefault()
            {
                Id = scheduleDefaultDto.Id.GetValueOrDefault(),
                Name = scheduleDefaultDto.Name,
                StartDate = scheduleDefaultDto.StartDate ?? throw new ArgumentException($"Schedule must have a start date"),
                EndDate = scheduleDefaultDto.EndDate ?? throw new ArgumentException($"Schedule must have an end date"),
            };
        }
    }
}
