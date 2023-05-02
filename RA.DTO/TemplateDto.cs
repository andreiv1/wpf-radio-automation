using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DTO
{
    public class TemplateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TemplateDto(string name)
        {
            Name = name;
        }

        public static TemplateDto FromEntity(Template entity)
        {
            return new TemplateDto(entity.Name) { Id = entity.Id, };
        }

        public static Template ToEntity(TemplateDto dto)
        {
            return new Template { Id = dto.Id, Name = dto.Name };
        }
    }
}
