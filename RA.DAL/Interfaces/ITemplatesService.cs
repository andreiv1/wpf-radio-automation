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
        Task AddClockToTemplate(ClockTemplateDTO clockTemplate);
        Task AddTemplate(TemplateDTO templateDto);
        IEnumerable<ClockTemplateDTO> GetClocksForTemplate(int templateId);
        Task<IEnumerable<ClockTemplateDTO>> GetClocksForTemplateAsync(int templateId);
        Task<TemplateDTO> GetTemplate(int templateId);
        Task<IEnumerable<TemplateDTO>> GetTemplatesAsync();
        Task<IEnumerable<TemplateClockDTO>> GetTemplatesForClockAsync(int clockId);
        Task UpdateClockInTemplate(ClockTemplateDTO clockTemplate);
        Task UpdateTemplate(TemplateDTO templateDto);
    }
}
