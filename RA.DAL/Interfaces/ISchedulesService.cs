﻿using RA.DTO.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DAL
{
    public interface ISchedulesService
    {
        IScheduleDTO GetScheduleByDate(DateTime date);
    }
}