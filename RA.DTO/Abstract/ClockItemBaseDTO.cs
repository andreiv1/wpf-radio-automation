using RA.Database.Models;
using RA.Database.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DTO.Abstract
{
    public abstract class ClockItemBaseDTO
    {
        public int Id { get; set; }
        public int OrderIndex { get; set; }
        public int? ClockId { get; set; }
        public Clock Clock { get; set; }
    }
}
