using CommunityToolkit.Mvvm.ComponentModel;
using RA.DTO.Abstract;
using RA.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.Planner.ViewModels.Playlists.Models
{
    public enum ScheduleType
    {
        Default,
        Planned
    }

    public enum ScheduleGenerationStatus
    {
        NoScheduleFound,
        NotGenerated,
        Generating,
        Generated,
        Error
    }
    public partial class ScheduleOverviewModel : ObservableObject
    {
        public DateTime Date { get; private set; }

        public ScheduleType Type { get; private set; }

        public String? TemplateName { get; private set; } 

        [ObservableProperty]
        private ScheduleGenerationStatus generationStatus = ScheduleGenerationStatus.NotGenerated;

        public String? ErrorMessage { get; set; }

        public static ScheduleOverviewModel FromDto(DateTime date, ScheduleBaseDto? dto)
        {
            ScheduleOverviewModel model = new();
            model.Date = date;

            if (dto.GetType() == typeof(ScheduleDefaultDto))
            {
                model.Type = ScheduleType.Default;
            }
            else if (dto.GetType() == typeof(PlannedScheduleDto))
            {
                model.Type = ScheduleType.Planned;
            }
            else throw new InvalidEnumArgumentException($"{dto.GetType()} is not the correct type.");
            //model.TemplateName = dto.TemplateDto?.Name ?? "-";
            model.TemplateName = "Not implemented";
            return model;
        }
    }
}
