﻿using RA.Database.Models.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Database.Models
{
    [Table("Clocks")]
    public class Clock : BaseModel
    {
        [Required]
        [MaxLength(100)]
        public String Name { get; set; }

        public ICollection<ClockItemBase> ClockItems { get; set; }

        public ICollection<ClockTemplate> ClockTemplates { get; set; }
    }
}
