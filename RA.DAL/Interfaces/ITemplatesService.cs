﻿using RA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DAL
{
    public interface ITemplatesService
    {
        Task AddTemplate(TemplateDTO templateDto);
        Task<TemplateDTO> GetTemplate(int templateId);
        Task<IEnumerable<TemplateDTO>> GetTemplatesAsync();
        Task<IEnumerable<TemplateClockDTO>> GetTemplatesForClockWithId(int clockId);
        Task UpdateTemplate(TemplateDTO templateDto);
    }
}
