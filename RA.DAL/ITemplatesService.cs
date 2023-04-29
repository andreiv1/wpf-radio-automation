using RA.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DAL
{
    public interface ITemplatesService
    {
        Task<IEnumerable<TemplateDto>> GetTemplatesAsync();
        Task<IEnumerable<TemplateClockDto>> GetTemplatesForClockWithId(int clockId);
    }
}
