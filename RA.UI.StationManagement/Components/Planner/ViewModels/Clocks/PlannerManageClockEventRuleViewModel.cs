using CommunityToolkit.Mvvm.ComponentModel;
using RA.DAL;
using RA.Database.Models.Enums;
using RA.DTO;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.Planner.ViewModels.Clocks
{
    public partial class PlannerManageClockEventRuleViewModel : DialogViewModelBase
    {
        public static List<string> Events => Enum.GetNames(typeof(EventType)).ToList();

        [ObservableProperty]
        private string? selectedEvent;

        [ObservableProperty]
        private string? eventLabel;

        [ObservableProperty]
        private TimeSpan eventStartTime;

        [ObservableProperty]
        private TimeSpan? estimatedDuration;

        private readonly IClocksService clocksService;
        private readonly int clockId;

        public PlannerManageClockEventRuleViewModel(IWindowService windowService,
                                                    IClocksService clocksService,
                                                    int clockId) : base(windowService)
        {
            this.clocksService = clocksService;
            this.clockId = clockId;
        }

        public async Task AddClockItem()
        {
            if (SelectedEvent == null) return;
            var newClockItem = new ClockItemEventDTO
            {
                OrderIndex = -1,
                ClockId = clockId,
                EventType = (EventType)Enum.Parse(typeof(EventType), SelectedEvent),
                EventLabel = EventLabel ?? "",
                EstimatedEventStart = EventStartTime,
                EstimatedEventDuration = EstimatedDuration,
            };

            await clocksService.AddClockItem(newClockItem);
        }

        protected override bool CanFinishDialog()
        {
            //TODO:validari
            return true;
        }
    }
}
