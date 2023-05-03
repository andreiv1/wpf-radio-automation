using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DTO
{
    public class ClockDto
    {
        public int? Id { get; set; }
        public String Name { get; set; }

        public ClockDto()
        {
            Name = string.Empty;    
        }

        public ClockDto(string name)
        {
            Name = name;
        }

        public static ClockDto FromEntity(Clock clock)
        {
            return new ClockDto { Id = clock.Id, Name = clock.Name, };
        }

        public static Clock ToEntity(ClockDto dto)
        {
            return new Clock { Id = dto.Id.GetValueOrDefault(), Name = dto.Name, };
        }
    }
}
