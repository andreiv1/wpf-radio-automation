using RA.Database.Models;
using RA.DTO.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DTO
{
    public class SchedulePlannedDTO : ScheduleBaseDTO, IScheduleDTO
    {
        public SchedulePlannedType Type { get; set; }
        public SchedulePlannedFrequency? Frequency { get; set; }
        public TemplateDTO? Template { get; set; }
        public Boolean? IsMonday { get; set; }
        public Boolean? IsTuesday { get; set; }
        public Boolean? IsWednesday { get; set; }
        public Boolean? IsThursday { get; set; }
        public Boolean? IsFriday { get; set; }
        public Boolean? IsSaturday { get; set; }
        public Boolean? IsSunday { get; set; }
        public static SchedulePlannedDTO FromEntity(SchedulePlanned entity)
        {

            return new SchedulePlannedDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Type = entity.Type,
                Frequency = entity.Frequency,
                Template = TemplateDTO.FromEntity(entity.Template),
                IsMonday = entity.IsMonday,
                IsTuesday = entity.IsTuesday,
                IsWednesday = entity.IsWednesday,
                IsThursday = entity.IsThursday,
                IsFriday = entity.IsFriday,
                IsSaturday = entity.IsSaturday,
                IsSunday = entity.IsSunday,
            };
        }
        public static SchedulePlanned ToEntity(SchedulePlannedDTO dto)
        {
            if (dto.Template == null)
            {
                throw new ArgumentException($"Item must have attached a template");
            }
            return new SchedulePlanned
            {
                Name = dto.Name,
                Type = dto.Type,
                StartDate = dto.StartDate.GetValueOrDefault(),
                EndDate = dto.EndDate.GetValueOrDefault(),
                Frequency = dto.Frequency,
                TemplateId = dto.Template.Id,
                IsMonday = dto.IsMonday,
                IsTuesday = dto.IsTuesday,
                IsWednesday = dto.IsWednesday,
                IsThursday = dto.IsThursday,
                IsFriday = dto.IsFriday,
                IsSaturday = dto.IsSaturday,
                IsSunday = dto.IsSunday,
            };
        }
    }
}
