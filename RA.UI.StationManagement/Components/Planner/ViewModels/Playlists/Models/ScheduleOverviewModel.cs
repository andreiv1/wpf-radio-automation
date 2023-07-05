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
        Empty,
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

        public static ScheduleOverviewModel FromDto(DateTime date, IScheduleDTO? dto)
        {
            ScheduleOverviewModel model = new();
            model.Date = date;
            if(dto == null)
            {
                model.Type = ScheduleType.Empty;
                model.TemplateName = "-";
            } else
            {
                if (dto is SchedulePlannedDTO dtoPlannedDTO)
                {
                    model.Type = ScheduleType.Planned;
                }
                else
                model.Type = ScheduleType.Default;
                model.TemplateName = dto.Template?.Name;
            }
            
            return model;
        }
    }
}
