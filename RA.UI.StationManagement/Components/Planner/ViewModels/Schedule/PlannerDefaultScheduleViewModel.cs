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
        private readonly IDefaultSchedulesService defaultSchedulesService;
        private readonly ITemplatesService templatesService;

        #region Properties
        public ObservableCollection<ScheduleDefaultDto> DefaultSchedules { get; private set; } = new(); 
        
        [ObservableProperty]
        private ScheduleDefaultDto? selectedDefaultSchedule;
        public ObservableCollection<DefaultScheduleItem> DefaultScheduleItemsForSelected { get; private set; } = new();

        [ObservableProperty]
        private DefaultScheduleItem? selectedDefaultScheduleItem;

        partial void OnSelectedDefaultScheduleChanged(ScheduleDefaultDto? value)
        {
            _ = LoadDefaultScheduleForSelectedInterval();
        }
        public ObservableCollection<TemplateDto> Templates { get; private set; } = new();
        #endregion

        

        #region Constructor
        public PlannerDefaultScheduleViewModel(IDispatcherService dispatcherService, IWindowService windowService,
            IDefaultSchedulesService defaultSchedulesService, ITemplatesService templatesService)
        {
            this.dispatcherService = dispatcherService;
            this.windowService = windowService;
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
                SaveSelectedDefaultTemplateCommand.NotifyCanExecuteChanged();
            }
        }

        [RelayCommand(CanExecute = nameof(CanSaveSelectedDefaultTemplate))]
        // Save the selected default template items
        private async void SaveSelectedDefaultTemplate()
        {
            //List<ScheduleDefaultDto> toAdd = DefaultScheduleItemsForSelectedInterval
            //    .Select(it => DefaultScheduleItem.ToDto(it))
            //    .ToList();
            throw new NotImplementedException();
            //var result = await defaultScheduleService.AddDefaultScheduleItemsAsync(toAdd, SelectedInterval!);
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
        }

        //DEBUG
        private bool CanAddNewDefaultSchedule()
        {
            return true;
        }


        #endregion
    }
}
