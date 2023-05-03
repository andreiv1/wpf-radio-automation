﻿using RA.Database.Models;
using RA.DTO.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DTO
{
    public class SchedulePlannedDto : ScheduleBaseDto
    {
        public SchedulePlannedType Type { get; set; }
        public SchedulePlannedFrequency? Frequency { get; set; }

        public static SchedulePlannedDto FromEntity(SchedulePlanned entity)
        {
            throw new NotImplementedException();
        }
    }
}