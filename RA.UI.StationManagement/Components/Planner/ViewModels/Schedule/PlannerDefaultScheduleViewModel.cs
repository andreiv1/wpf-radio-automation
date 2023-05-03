using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.DAL;
using RA.DAL.Models;
using RA.Database.Models;
using RA.DTO;
using RA.Logic;
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
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.Planner.ViewModels.Schedule
{
    public partial class PlannerDefaultScheduleViewModel : ViewModelBase
    {
        private readonly IDispatcherService dispatcherService;
        private readonly IWindowService windowService;
        private readonly IMessageBoxService messageBoxService;
        private readonly IDefaultSchedulesService defaultSchedulesService;
        private readonly ITemplatesService templatesService;

        #region Properties
        public ObservableCollection<ScheduleDefaultDto> DefaultSchedules { get; private set; } = new(); 
        
        [ObservableProperty]
        private ScheduleDefaultDto? selectedDefaultSchedule;
        public ObservableCollection<DefaultScheduleItem> DefaultScheduleItemsForSelected { get; private set; } = new();

        [ObservableProperty]
        private DefaultScheduleItem? selectedDefaultScheduleItem;

        /// <summary>
        /// Used for creating a SelectedDefaultSchedule
        /// </summary>
        [ObservableProperty]
        private DateTimeRange addNewScheduleRange = new DateTimeRange(DateTime.Now.Date, DateTime.Now.Date.AddDays(7));

        partial void OnSelectedDefaultScheduleChanged(ScheduleDefaultDto? value)
        {
            _ = LoadDefaultScheduleForSelectedInterval();
        }
        public ObservableCollection<TemplateDto> Templates { get; private set; } = new();
        #endregion

        

        #region Constructor
        public PlannerDefaultScheduleViewModel(IDispatcherService dispatcherService, IWindowService windowService, IMessageBoxService messageBoxService,
            IDefaultSchedulesService defaultSchedulesService, ITemplatesService templatesService)
        {
            this.dispatcherService = dispatcherService;
            this.windowService = windowService;
            this.messageBoxService = messageBoxService;
            this.defaultSchedulesService = defaultSchedulesService;
            this.templatesService = templatesService;
            _ = LoadDefaultSchedules();
        }

        #endregion

        #region Data fetching
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
                    ScheduleDefaultItemDto? item = schedule[day];
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
                ScheduleDefaultItemDto? item = schedule[day];
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
        #endregion

        #region Commands

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
                //Add new schedule
                SelectedDefaultSchedule.Items = DefaultScheduleItemsForSelected.Select(s => DefaultScheduleItem.ToDto(s, SelectedDefaultSchedule)).ToList();
                await defaultSchedulesService.AddDefaultSchedule(SelectedDefaultSchedule);
                messageBoxService.ShowInfo($"New default schedule added succesfully.");

                //Refresh the schedules
                await LoadDefaultSchedules();

                SelectedDefaultSchedule = null;
                DefaultScheduleItemsForSelected.Clear();
            }
            else
            {
                //Update existing schedule items
                List<ScheduleDefaultItemDto> toAdd = DefaultScheduleItemsForSelected.Where(s => !s.Id.HasValue)
                    .Select(s => DefaultScheduleItem.ToDto(s, SelectedDefaultSchedule))
                    .ToList();
                List<ScheduleDefaultItemDto> toUpdate = DefaultScheduleItemsForSelected.Where(s => s.IsUpdated)
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
                StartDate = AddNewScheduleRange.StartDate,
                EndDate = AddNewScheduleRange.EndDate
            };
        }

        //DEBUG
        private bool CanAddNewDefaultSchedule()
        {
            return true;
        }


        #endregion
    }
}
