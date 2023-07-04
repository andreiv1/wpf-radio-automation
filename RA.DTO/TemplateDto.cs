using RA.Database.Models;

namespace RA.DTO
{
    public class TemplateDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public TemplateDTO(string name)
        {
            Name = name;
        }

        public TemplateDTO(int id)
        {
            Id = id;
        }

        public static TemplateDTO FromEntity(Template entity)
        {
            return new TemplateDTO(entity.Name) { Id = entity.Id, };
        }

        public static Template ToEntity(TemplateDTO dto)
        {
            return new Template 
            { 
                Id = dto.Id, 
                Name = dto.Name,
            };
        }
    }
}
