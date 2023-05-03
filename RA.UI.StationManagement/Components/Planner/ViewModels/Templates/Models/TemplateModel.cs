using CommunityToolkit.Mvvm.ComponentModel;
using RA.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.Planner.ViewModels.Templates.Models
{
    public class TemplateModel : ObservableValidator
    {
        public int? Id { get; set; }

        private string name = "";

        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value, true);
        }

        public static TemplateModel FromDto(TemplateDTO dto)
        {
            return new TemplateModel() { Id = dto.Id, Name = dto.Name };

        }

        public static TemplateDTO ToDto(TemplateModel template)
        {
            return new TemplateDTO(template.Name) { Id = template.Id.GetValueOrDefault(),  };
        }
    }
}
