using RA.DTO;
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
        Task DeleteClockInTemplate(int templateId, int clockId, TimeSpan startTime);
        IEnumerable<ClockTemplateDTO> GetClocksForTemplate(int templateId);
        Task<IEnumerable<ClockTemplateDTO>> GetClocksForTemplateAsync(int templateId);
        Task<TemplateDTO> GetTemplate(int templateId);
        Task<IEnumerable<TemplateDTO>> GetTemplatesAsync(string? searchQuery = null);
        Task<IEnumerable<TemplateClockDTO>> GetTemplatesForClockAsync(int clockId);
        Task UpdateClockInTemplate(TimeSpan oldStartTime, ClockTemplateDTO clockTemplate);
        Task UpdateTemplate(TemplateDTO templateDto);
    }
}
