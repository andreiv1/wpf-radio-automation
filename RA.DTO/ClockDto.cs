using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Dto
{
    public class ClockDto
    {
        public int Id { get; set; }
        public String Name { get; set; }

        public static ClockDto FromEntity(Clock clock)
        {
            return new ClockDto { Id = clock.Id, Name = clock.Name, };
        }
    }
}
