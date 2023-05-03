using CommunityToolkit.Mvvm.ComponentModel;
using RA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.Planner.ViewModels.MainContent.Models
{
    public partial class ClockModel : ObservableObject
    {
        public int Id { get; set; }
        [ObservableProperty]
        private string name = string.Empty;

        public static ClockModel FromDto(DTO.ClockDto dto)
        {
            return new ClockModel()
            {
                Id = dto.Id.GetValueOrDefault(),
                Name = dto.Name
            };
        }
    }
}
