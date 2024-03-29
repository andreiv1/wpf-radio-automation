﻿using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DTO
{
    public class TemplateClockDTO
    {
        public int ClockId { get; set; }
        public int TemplateId { get; set; }
        public TimeSpan StartTime { get; set; }
        public int ClockSpan { get; set; }

        public String ClockName { get; set; }

        public static TemplateClockDTO FromEntity(ClockTemplate templateClock)
        {
            return new TemplateClockDTO { 
                ClockId = templateClock.ClockId, 
                TemplateId = templateClock.TemplateId, 
                StartTime = templateClock.StartTime, 
                ClockSpan = templateClock.ClockSpan,
                ClockName = templateClock.Clock.Name
            };
        }
    }
}
