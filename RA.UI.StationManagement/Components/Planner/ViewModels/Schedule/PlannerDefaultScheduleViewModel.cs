using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.DAL;
using RA.DTO;
using RA.UI.Core.Services;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.Planner.ViewModels.Schedule.Models;
using RA.UI.StationManagement.Dialogs.TemplateSelectDialog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.Planner.ViewModels.Schedule
{
    public partial class PlannerDefaultScheduleViewModel : ViewModelBase
    {
        private readonly IDispatcherService dispatcherService;
        private readonly IWindowService windowService;
        private readonly IMessageBoxService messageBoxService;
        private readonly ISchedulesDefaultService defaultSchedulesService;
        private readonly ITemplatesService templatesService;

       #region Properties
        public ObservableCollection<ScheduleDefaultDTO> DefaultSchedules { get; private set; } = new(); 
        
        [ObservableProperty]
        private ScheduleDefaultDTO? selectedDefaultSchedule;
        public ObservableCollection<DefaultScheduleItem> DefaultScheduleItemsForSelected { get; private set; } = new();

        [ObservableProperty]
        private DefaultScheduleItem? selectedDefaultScheduleItem;


        [ObservableProperty]
        private DateTime newScheduleStartDate = DateTime.Now.Date;

        partial void OnNewScheduleStartDateChanged(DateTime oldValue, DateTime newValue)
        {
            if(newValue >= NewScheduleEndDate)
            {
                messageBoxService.ShowWarning($"Start date must be less than end date!");
                NewScheduleStartDate = oldValue;
                return;
            }
            var overlapTask = Task.Run(async () => {
                IsAnyOverLap = await CheckOverlaping(newValue, NewScheduleEndDate);
            });

            overlapTask.ContinueWith(async (t) => await dispatcherService.InvokeOnUIThreadAsync(() => AddNewDefaultScheduleCommand.NotifyCanExecuteChanged()));

        }
    

        [ObservableProperty]
        private DateTime newScheduleEndDate = DateTime.Now.Date.AddDays(7);

        partial void OnNewScheduleEndDateChanged(DateTime oldValue, DateTime newValue)
        {
            if(newValue <= NewScheduleStartDate)
            {
                messageBoxService.ShowWarning($"End date must be greater than start date!");
                NewScheduleEndDate = oldValue;
                return;
            }
            var overlapTask = Task.Run(async () => {
                IsAnyOverLap = await CheckOverlaping(NewScheduleStartDate, newValue);
            });

            overlapTask.ContinueWith(async (t) => await dispatcherService.InvokeOnUIThreadAsync(() => AddNewDefaultScheduleCommand.NotifyCanExecuteChanged()));
        }



        [ObservableProperty]
        private string newScheduleName;

        [ObservableProperty]
        private bool isAnyOverLap = false;

        partial void OnSelectedDefaultScheduleChanged(ScheduleDefaultDTO? value)
        {
            _ = LoadDefaultScheduleForSelectedInterval();
        }
        public ObservableCollection<TemplateDTO> Templates { get; private set; } = new();
    

      
        public PlannerDefaultScheduleViewModel(IDispatcherService dispatcherService, IWindowService windowService, IMessageBoxService messageBoxService,
            ISchedulesDefaultService defaultSchedulesService, ITemplatesService templatesService)
        {
            this.dispatcherService = dispatcherService;
            this.windowService = windowService;
            this.messageBoxService = messageBoxService;
            this.defaultSchedulesService = defaultSchedulesService;
            this.templatesService = templatesService;
            _ = LoadDefaultSchedules();
        }

        //Data fetching
        private async Task LoadDefaultSchedules()
        {
            DefaultSchedules.Clear();
            await Task.Run(() =>
            {
                foreach (var schedule in defaultSchedulesService.GetDefaultSchedules())
                {
                    dispatcherService.InvokeOnUIThread(() =>
                    {
                        DefaultSchedules.Add(schedule);
                    });
                }
            });
            
        }

        private async Task LoadDefaultScheduleForSelectedInterval()
        {
            if (SelectedDefaultSchedule == null || SelectedDefaultSchedule.Id == null) return;
            DefaultScheduleItemsForSelected.Clear();
            var schedule = await defaultSchedulesService.GetDefaultScheduleItems(SelectedDefaultSchedule);
            DayOfWeek firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            for (DayOfWeek day = firstDayOfWeek; day <= DayOfWeek.Saturday; day++)
            {
                if (schedule.ContainsKey(day))
                {
                    ScheduleDefaultItemDTO? item = schedule[day];
                    if (item != null && item.Template != null)
                    {
                        DefaultScheduleItemsForSelected.Add(
                            new DefaultScheduleItem()
                            {
                                Id = item.Id.GetValueOrDefault(),
                                TemplateId = item.Template.Id,
                                TemplateName = item.Template?.Name ?? "",
                                Day = day,
                            });
                    }
                    else
                    {
                        DefaultScheduleItemsForSelected.Add(
                        new DefaultScheduleItem()
                        {
                            Day = day,
                        });
                    }
                }
            }

            if(firstDayOfWeek == DayOfWeek.Monday)
            {
                var day = DayOfWeek.Sunday;
                ScheduleDefaultItemDTO? item = schedule[day];
                if (item != null && item.Template != null)
                {
                    DefaultScheduleItemsForSelected.Add(
                        new DefaultScheduleItem()
                        {
                            Id = item.Id.GetValueOrDefault(),
                            TemplateId = item.Template.Id,
                            TemplateName = item.Template?.Name ?? "",
                            Day = day,
                        });
                }
                else
                {
                    DefaultScheduleItemsForSelected.Add(
                      new DefaultScheduleItem()
                      {
                          Day = day,
                      });
                }
            }

 
            SaveSelectedDefaultTemplateCommand.NotifyCanExecuteChanged();
        }

        private async Task LoadTemplates()
        {
            var templates = await templatesService.GetTemplatesAsync();
            foreach (var template in templates)
            {
                Templates.Add(template);
            }
        }

        private async Task<bool> CheckOverlaping(DateTime start, DateTime end)
        {
            return !(await defaultSchedulesService.IsAnyOverlap(start, end));
        }
        

        //Commands

        //Commands for selected default schedule
        [RelayCommand]
        private void SelectTemplateForDay()
        {
            var templateSelectDialog = windowService.ShowDialog<TemplateSelectViewModel>();
            if (SelectedDefaultScheduleItem != null &&  templateSelectDialog.SelectedTemplate != null)
            {
                SelectedDefaultScheduleItem.TemplateName = templateSelectDialog.SelectedTemplate.Name;
                SelectedDefaultScheduleItem.TemplateId = templateSelectDialog.SelectedTemplate.Id;
                SelectedDefaultScheduleItem.IsUpdated = true;
                SaveSelectedDefaultTemplateCommand.NotifyCanExecuteChanged();
            }
        }


        [RelayCommand(CanExecute = nameof(CanSaveSelectedDefaultTemplate))]
        private async void SaveSelectedDefaultTemplate()
        {
            if (SelectedDefaultSchedule == null) return;

            if (SelectedDefaultSchedule.Id == null)
            {
                if(string.IsNullOrEmpty(NewScheduleName))
                {
                    messageBoxService.ShowWarning($"The default schedule must have a name.");
                    return;
                }
                //Add new schedule
                SelectedDefaultSchedule.Items = DefaultScheduleItemsForSelected.Select(s => DefaultScheduleItem.ToDto(s, SelectedDefaultSchedule)).ToList();
                await defaultSchedulesService.AddDefaultSchedule(SelectedDefaultSchedule);

                //Because i added the schedule, the interval is now busy
                IsAnyOverLap = true;
                messageBoxService.ShowInfo($"New default schedule added succesfully.");

                //Refresh the schedules
                await LoadDefaultSchedules();

                SelectedDefaultSchedule = null;
                DefaultScheduleItemsForSelected.Clear();
            }
            else
            {
                //Update existing schedule items
                List<ScheduleDefaultItemDTO> toAdd = DefaultScheduleItemsForSelected.Where(s => !s.Id.HasValue)
                    .Select(s => DefaultScheduleItem.ToDto(s, SelectedDefaultSchedule))
                    .ToList();
                List<ScheduleDefaultItemDTO> toUpdate = DefaultScheduleItemsForSelected.Where(s => s.IsUpdated)
                    .Select(s => DefaultScheduleItem.ToDto(s, SelectedDefaultSchedule))
                    .ToList();

                int addedNo = await defaultSchedulesService.UpdateDefaultScheduleItems(toAdd);
                int updatedNo = await defaultSchedulesService.UpdateDefaultScheduleItems(toUpdate);

                _ = LoadDefaultScheduleForSelectedInterval();

                if (addedNo > 0 && updatedNo > 0)
                {
                    messageBoxService.ShowInfo($"{addedNo} {(addedNo == 1 ? "day" : "days")} were added and {updatedNo} {(updatedNo == 1 ? "day" : "days")} were updated in the active default schedule.");
                }
                else
                if (updatedNo > 0)
                {
                    messageBoxService.ShowInfo($"{updatedNo} {(updatedNo == 1 ? "day" : "days")} were updated in the active default schedule.");
                }
                else
                if (addedNo > 0)
                {
                    messageBoxService.ShowInfo($"{addedNo} {(addedNo == 1 ? "day" : "days")} were added in the active default schedule.");
                }
            }

        }

        private bool CanSaveSelectedDefaultTemplate()
        {
            if(DefaultScheduleItemsForSelected.Count == 0)
            {
                return false;
            }
            foreach(var item in DefaultScheduleItemsForSelected)
            {
                if(item.TemplateId == 0)
                {
                    return false;
                }
            }
            return true;
        }

        [RelayCommand(CanExecute = nameof(CanAddNewDefaultSchedule))]
        private void AddNewDefaultSchedule()
        {
            if (string.IsNullOrEmpty(NewScheduleName))
            {
                messageBoxService.ShowWarning($"The default schedule must have a name.");
                return;
            }

            DefaultScheduleItemsForSelected.Clear();
            var firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            int startDayIndex = 0;

            if (firstDayOfWeek == DayOfWeek.Monday)
            {
                startDayIndex = 1;
            }

            for (int i = startDayIndex; i < 7; i++)
            {
                DefaultScheduleItemsForSelected.Add(
                           new DefaultScheduleItem()
                           {
                               Day = (DayOfWeek)i
                           });


            }

            if (firstDayOfWeek == DayOfWeek.Monday)
            {
                DefaultScheduleItemsForSelected.Add(
                           new DefaultScheduleItem()
                           {
                               Day = DayOfWeek.Sunday,
                           });
            }
            SelectedDefaultSchedule = new()
            {
                Name = NewScheduleName,
                StartDate = NewScheduleStartDate,
                EndDate = NewScheduleEndDate,
            };
        }

        private bool CanAddNewDefaultSchedule()
        {
            return IsAnyOverLap;
        }


        #endregion
    }
}
