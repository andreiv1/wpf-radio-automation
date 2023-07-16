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
        private readonly IMessageBoxService messageBoxService;
        private readonly IDispatcherService dispatcherService;
        private readonly ITemplatesService templatesService;
        private readonly ISchedulesPlannedService schedulesPlannedService;
        private readonly int? plannedScheduleId;
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(FinishDialogCommand))]
        private SchedulePlannedType scheduleType = SchedulePlannedType.OneTime;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(FinishDialogCommand))]
        private String name = string.Empty;

        [ObservableProperty]
        private DateTime startDate = DateTime.Today.Date;

        partial void OnStartDateChanged(DateTime oldValue, DateTime newValue)
        {
            _ = CheckOneTimeOverlapping(oldValue, newValue);
        }

        private async Task CheckOneTimeOverlapping(DateTime oldValue, DateTime newValue)
        {
            if (ScheduleType == SchedulePlannedType.OneTime)
            {
                bool isOverlapping = await schedulesPlannedService.IsAnyOverlap(ScheduleType, StartDate, excludeScheduleId: plannedScheduleId);
                if (isOverlapping)
                {
                    messageBoxService.ShowWarning("There is already a one time schedule planned for this date");
                    return;
                }

            }
            throw new Exception("Schedule type is not one time");
        }

        [ObservableProperty]
        private DateTime endDate = DateTime.Today.Date.AddDays(1);

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(FinishDialogCommand))]
        private SchedulePlannedFrequency selectedFrequency;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(FinishDialogCommand))]
        private TemplateDTO? selectedTemplate;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(FinishDialogCommand))]
        private bool isMonday;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(FinishDialogCommand))]
        private bool isTuesday;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(FinishDialogCommand))]
        private bool isWednesday;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(FinishDialogCommand))]
        private bool isThursday;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(FinishDialogCommand))]
        private bool isFriday;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(FinishDialogCommand))]
        private bool isSaturday;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(FinishDialogCommand))]
        private bool isSunday;

        public bool IsEditing => plannedScheduleId.HasValue;

        public List<SchedulePlannedFrequency> ScheduleFrequencies => scheduleFrequencies.ToList();

        public ObservableCollection<TemplateDTO> Templates { get; set; } = new();

        public PlannerManageScheduleItemViewModel(IWindowService windowService,
                                                  IMessageBoxService messageBoxService,
                                                  IDispatcherService dispatcherService,
                                                  ITemplatesService templatesService,
                                                  ISchedulesPlannedService schedulesPlannedService) : base(windowService)
        {
            DialogName = "Add new planned schedule";
            this.messageBoxService = messageBoxService;
            this.dispatcherService = dispatcherService;
            this.templatesService = templatesService;
            this.schedulesPlannedService = schedulesPlannedService;
            _ = LoadTemplates();
        }

        public PlannerManageScheduleItemViewModel(IWindowService windowService,
                                                  IMessageBoxService messageBoxService,
                                                  IDispatcherService dispatcherService,
                                                  ITemplatesService templatesService,
                                                  ISchedulesPlannedService schedulesPlannedService,
                                                  int plannedScheduleId) : this(windowService, messageBoxService, dispatcherService, templatesService, schedulesPlannedService)
        {
            DialogName = "Edit planned schedule";
            this.plannedScheduleId = plannedScheduleId;
            _ = LoadSchedule();
            
        }

        private async Task LoadSchedule()
        {
            var result = await schedulesPlannedService.GetSchedule(plannedScheduleId.GetValueOrDefault());
            Name = result?.Name ?? "";
            ScheduleType = result.Type;
            SelectedFrequency = result.Frequency.GetValueOrDefault();
            StartDate = result.StartDate.GetValueOrDefault();
            EndDate = result.EndDate.GetValueOrDefault();
            if (result.Type == SchedulePlannedType.Recurrent)
            {
                IsMonday = result.IsMonday.GetValueOrDefault();
                IsTuesday = result.IsTuesday.GetValueOrDefault();
                IsWednesday = result.IsWednesday.GetValueOrDefault();
                IsThursday = result.IsThursday.GetValueOrDefault();
                IsFriday = result.IsFriday.GetValueOrDefault();
                IsSaturday = result.IsSaturday.GetValueOrDefault();
                IsSunday = result.IsSunday.GetValueOrDefault();   
            }
            SelectedTemplate = Templates.FirstOrDefault(t => t.Id == result.Template?.Id);
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
            if(ScheduleType == SchedulePlannedType.Recurrent)
            {
                if(StartDate > EndDate)
                {
                    messageBoxService.ShowWarning($"The start date can't be greater than end date!");
                    return;
                }
                if (!CheckDaysExistInRange())
                {
                    messageBoxService.ShowWarning($"The selected days must be contained in the date range!");
                    return;
                }

                if (ScheduleType == SchedulePlannedType.OneTime)
                {
                    bool isOverlapping = await schedulesPlannedService.IsAnyOverlap(ScheduleType, StartDate, excludeScheduleId: plannedScheduleId);
                    if (isOverlapping)
                    {
                        messageBoxService.ShowWarning("There is already a one time schedule planned for this date");
                        return;
                    }
                }
                else
                {
                    bool isOverlapping = await schedulesPlannedService.IsAnyOverlap(ScheduleType, StartDate, EndDate, excludeScheduleId: plannedScheduleId);
                    if (isOverlapping)
                    {
                        messageBoxService.ShowWarning("There is already a recurrent schedule in the selected range");
                        return;
                    }
                }
            }
            else if(ScheduleType == SchedulePlannedType.OneTime)
            {
                bool isOverlap = await schedulesPlannedService.IsAnyOverlap(ScheduleType, StartDate);
                if (isOverlap)
                {
                    messageBoxService.ShowError($"The selected date overlaps with another schedule!");
                    return;
                }
            }
           
            await SaveSchedule();
            base.FinishDialog();
        }

        private async Task SaveSchedule()
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

            await Task.Run(() => { 
                if(IsEditing && plannedScheduleId != null)
                {
                    dto.Id = plannedScheduleId.Value;
                    schedulesPlannedService.UpdatePlannedSchedule(dto);
                }
                    
                else schedulesPlannedService.AddPlannedSchedule(dto); });
        }

 
        protected override bool CanFinishDialog()
        {
            if (string.IsNullOrEmpty(Name)) return false;
            if (SelectedTemplate == null) return false;  
            if (ScheduleType == SchedulePlannedType.OneTime)
            {
               
            } else if(ScheduleType == SchedulePlannedType.Recurrent)
            {
                if (!(IsMonday || IsTuesday || IsWednesday || IsThursday || IsFriday || IsSaturday || IsSunday))
                    return false;

            }
            return true;
        }

        private bool CheckDaysExistInRange()
        {
            DateTime currentDate = StartDate;
            while (currentDate <= EndDate)
            {
                if ((currentDate.DayOfWeek == DayOfWeek.Monday && IsMonday) ||
                        (currentDate.DayOfWeek == DayOfWeek.Tuesday && IsTuesday) ||
                        (currentDate.DayOfWeek == DayOfWeek.Wednesday && IsWednesday) ||
                        (currentDate.DayOfWeek == DayOfWeek.Thursday && IsThursday) ||
                        (currentDate.DayOfWeek == DayOfWeek.Friday && IsFriday) ||
                        (currentDate.DayOfWeek == DayOfWeek.Saturday && IsSaturday) ||
                        (currentDate.DayOfWeek == DayOfWeek.Sunday && IsSunday))
                    {
                    // At least one checked day falls within the interval
                    return true;
                }

                currentDate = currentDate.AddDays(1);
            }

            // No Monday to Sunday within the interval
            return false;
        }
    }
}
