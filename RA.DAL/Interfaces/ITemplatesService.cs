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
        Task AddTemplate(TemplateDto templateDto);
        Task<TemplateDto> GetTemplate(int templateId);
        Task<IEnumerable<TemplateDto>> GetTemplatesAsync();
        Task<IEnumerable<TemplateClockDto>> GetTemplatesForClockWithId(int clockId);
        Task UpdateTemplate(TemplateDto templateDto);
    }
}
