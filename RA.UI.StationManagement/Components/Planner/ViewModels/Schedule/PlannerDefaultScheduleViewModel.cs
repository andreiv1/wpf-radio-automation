using CommunityToolkit.Mvvm.ComponentModel;
using RA.DAL;
using RA.DAL.Models;
using RA.DTO;
using RA.UI.Core.Services;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.Planner.ViewModels.Schedule.Models;
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
        private readonly IDefaultScheduleService defaultScheduleService;
        private readonly ITemplatesService templatesService;

        public ObservableCollection<DateTimeRange> DefaultIntervals { get; private set; } = new();

        [ObservableProperty]
        private DateTimeRange? selectedInterval;

        public ObservableCollection<DefaultScheduleItem> DefaultScheduleItemsForSelectedInterval { get; private set; } = new();
        public ObservableCollection<TemplateDto> Templates { get; private set; } = new();

        partial void OnSelectedIntervalChanged(DateTimeRange? value)
        {
            _ = LoadDefaultScheduleForSelectedInterval();
            _ = LoadTemplates();
        }

        #region Constructor
        public PlannerDefaultScheduleViewModel(IDispatcherService dispatcherService,
            IDefaultScheduleService defaultScheduleService, ITemplatesService templatesService)
        {
            this.dispatcherService = dispatcherService;
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
    }
}
