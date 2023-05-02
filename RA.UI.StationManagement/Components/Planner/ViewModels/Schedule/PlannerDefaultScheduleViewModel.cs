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
        private readonly IDefaultScheduleService defaultScheduleService;
        private readonly ITemplatesService templatesService;

        public ObservableCollection<DateTimeRange> DefaultIntervals { get; private set; } = new();

        [ObservableProperty]
        private DateTimeRange? addNewScheduleRange;

        [ObservableProperty]
        private DateTimeRange? selectedInterval;

        public ObservableCollection<DefaultScheduleItem> DefaultScheduleItemsForSelectedInterval { get; private set; } = new();

        [ObservableProperty]
        private DefaultScheduleItem? selectedDefaultScheduleItem;
        public ObservableCollection<TemplateDto> Templates { get; private set; } = new();

        partial void OnSelectedIntervalChanged(DateTimeRange? value)
        {
            _ = LoadDefaultScheduleForSelectedInterval();
            _ = LoadTemplates();
        }

        #region Constructor
        public PlannerDefaultScheduleViewModel(IDispatcherService dispatcherService, IWindowService windowService,
            IDefaultScheduleService defaultScheduleService, ITemplatesService templatesService)
        {
            this.dispatcherService = dispatcherService;
            this.windowService = windowService;
            this.defaultScheduleService = defaultScheduleService;
            this.templatesService = templatesService;
            _ = LoadIntervals();
        }

        #endregion

        #region Data fetching
        private async Task LoadIntervals()
        {
            var intervals = await Task.Run(() => defaultScheduleService.GetDefaultSchedulesRangeAsync(0, 500));
            DefaultIntervals.Clear();
            foreach(var interval in intervals)
            {
                DefaultIntervals.Add(interval);                
            }
        }

        private async Task LoadDefaultScheduleForSelectedInterval()
        {
            if (SelectedInterval == null) return;
            DefaultScheduleItemsForSelectedInterval.Clear();
            var schedule = await Task.Run(() =>
                defaultScheduleService.GetDefaultScheduleWithTemplateAsync(SelectedInterval));


            var firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            int startDayIndex = 0;

            if (firstDayOfWeek == DayOfWeek.Monday)
            {
                startDayIndex = 1;
            }

            for (int i = startDayIndex; i < 7; i++)
            {
                if (schedule.ContainsKey((DayOfWeek)i))
                {
                    DefaultScheduleDto item = schedule[(DayOfWeek)i];
                    if (item is not null)
                    {
                        DefaultScheduleItemsForSelectedInterval.Add(
                            new DefaultScheduleItem()
                            {
                                Id = (int)item.Id,
                                TemplateId = item.TemplateDto.Id,
                                TemplateName = item.TemplateDto.Name,
                                Day = (DayOfWeek)i,
                            });
                    }
                    else
                    {
                        DefaultScheduleItemsForSelectedInterval.Add(
                            new DefaultScheduleItem()
                            {
                                Day = (DayOfWeek)i
                            });
                    }
                } 


            }

            if (firstDayOfWeek == DayOfWeek.Monday)
            {
                DefaultScheduleDto item = schedule[DayOfWeek.Sunday];
                if (item is not null)
                {
                    DefaultScheduleItemsForSelectedInterval.Add(
                        new DefaultScheduleItem()
                        {
                            Id = (int)item.Id,
                            TemplateId = item.TemplateDto.Id,
                            TemplateName = item.TemplateDto.Name,
                            Day = DayOfWeek.Sunday,
                        });
                }
                else
                {
                    DefaultScheduleItemsForSelectedInterval.Add(
                        new DefaultScheduleItem()
                        {
                            Day = DayOfWeek.Sunday,
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
            List<DefaultScheduleDto> toAdd = DefaultScheduleItemsForSelectedInterval
                .Select(it => DefaultScheduleItem.ToDto(it))
                .ToList();

            var result = await defaultScheduleService.AddDefaultScheduleItemsAsync(toAdd, SelectedInterval!);
        }

        private bool CanSaveSelectedDefaultTemplate()
        {
            if(DefaultScheduleItemsForSelectedInterval.Count == 0)
            {
                return false;
            }
            foreach(var item in DefaultScheduleItemsForSelectedInterval)
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
            DefaultScheduleItemsForSelectedInterval.Clear();
            var firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            int startDayIndex = 0;

            if (firstDayOfWeek == DayOfWeek.Monday)
            {
                startDayIndex = 1;
            }

            for (int i = startDayIndex; i < 7; i++)
            {
                DefaultScheduleItemsForSelectedInterval.Add(
                           new DefaultScheduleItem()
                           {
                               Day = (DayOfWeek)i
                           });


            }

            if (firstDayOfWeek == DayOfWeek.Monday)
            {
                DefaultScheduleItemsForSelectedInterval.Add(
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
