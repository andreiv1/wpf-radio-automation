using CommunityToolkit.Mvvm.ComponentModel;
using RA.DAL;
using RA.Database.Models;
using RA.DTO;
using RA.UI.Core.Services;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using Syncfusion.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RA.UI.StationManagement.Components.Planner.ViewModels.Schedule
{
    public partial class PlannerManageScheduleItemViewModel : DialogViewModelBase
    {
        private static IEnumerable<SchedulePlannedFrequency> scheduleFrequencies = (IEnumerable<SchedulePlannedFrequency>)
            Enum.GetValues(typeof(SchedulePlannedFrequency)).Cast<SchedulePlannedType>();

        private readonly IDispatcherService dispatcherService;
        private readonly ITemplatesService templatesService;
        private readonly ISchedulesPlannedService schedulesPlannedService;
        [ObservableProperty]
        private SchedulePlannedType scheduleType = SchedulePlannedType.Recurrent;

        [ObservableProperty]
        private String name = string.Empty;

        [ObservableProperty]
        private DateTime startDate = DateTime.Today.Date;

        [ObservableProperty]
        private DateTime endDate = DateTime.Today.Date.AddDays(1);

        [ObservableProperty]
        private SchedulePlannedFrequency selectedFrequency;

        [ObservableProperty]
        private TemplateDTO? selectedTemplate;

        [ObservableProperty]
        private bool isMonday;

        [ObservableProperty]
        private bool isTuesday;

        [ObservableProperty]
        private bool isWednesday;

        [ObservableProperty]
        private bool isThursday;

        [ObservableProperty]
        private bool isFriday;

        [ObservableProperty]
        private bool isSaturday;

        [ObservableProperty]
        private bool isSunday;

        public List<SchedulePlannedFrequency> ScheduleFrequencies => scheduleFrequencies.ToList();

        public ObservableCollection<TemplateDTO> Templates { get; set; } = new();

        public PlannerManageScheduleItemViewModel(IWindowService windowService,
                                                  IDispatcherService dispatcherService,
                                                  ITemplatesService templatesService,
                                                  ISchedulesPlannedService schedulesPlannedService) : base(windowService)
        {
            this.dispatcherService = dispatcherService;
            this.templatesService = templatesService;
            this.schedulesPlannedService = schedulesPlannedService;
            _ = LoadTemplates();
        }

        private async Task LoadTemplates()
        {
            var templates = await Task.Run(() => templatesService.GetTemplatesAsync());
            Templates.Clear();
            foreach (var template in templates)
            {
                dispatcherService.InvokeOnUIThread(() => Templates.Add(template));
            }
        }

        protected override async void FinishDialog()
        {
            await AddSchedule();
            base.FinishDialog();
        }

        private async Task AddSchedule()
        {
            SchedulePlannedDTO dto = new()
            {
                Name = Name,
                Type = ScheduleType,
                Template = SelectedTemplate,
                StartDate = StartDate,
            };

            if (ScheduleType == SchedulePlannedType.Recurrent)
            {
                dto.EndDate = EndDate;
                dto.Frequency = SelectedFrequency;
                dto.IsMonday = IsMonday;
                dto.IsTuesday = IsTuesday;
                dto.IsWednesday = IsWednesday;
                dto.IsThursday = IsThursday;
                dto.IsFriday = IsFriday;
                dto.IsSaturday = IsSaturday;
                dto.IsSunday = IsSunday;
            }

            await Task.Run(() => { schedulesPlannedService.AddPlannedSchedule(dto); });
        }

        protected override bool CanFinishDialog()
        {
            return true;
        }
    }
}
