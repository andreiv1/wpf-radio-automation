using RA.DTO.Abstract;
using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DTO
{
    public class ScheduleDefaultDto : ScheduleBaseDto
    {
        public ICollection<ScheduleDefaultItemDto>? Items { get; set; }

        public static ScheduleDefaultDto FromEntity(ScheduleDefault entity)
        {
            var dto = new ScheduleDefaultDto()
            {
                Id = entity.Id,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Name = entity.Name,

            };
            dto.Items = entity.ScheduleDefaultItems?
                    .Select(sd => ScheduleDefaultItemDto.FromEntity(sd, dto))
                    .ToList();
            return dto;
        }

        public static ScheduleDefault ToEntity(ScheduleDefaultDto scheduleDefaultDto)
        {
            return new ScheduleDefault()
            {
                Id = scheduleDefaultDto.Id.GetValueOrDefault(),
                StartDate = scheduleDefaultDto.StartDate ?? throw new ArgumentException($"Schedule must have a start date"),
                EndDate = scheduleDefaultDto.EndDate ?? throw new ArgumentException($"Schedule must have an end date"),
            };
        }
    }
}
