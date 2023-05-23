using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.DAL;
using RA.DTO;
using RA.UI.Core.Services;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.Planner.ViewModels.Templates
{
    public partial class PlannerTemplateSelectClockViewModel : DialogViewModelBase
    {
        private readonly IDispatcherService dispatcherService;
        private readonly IClocksService clocksService;

        public ObservableCollection<ClockDTO> Clocks { get; set; } = new();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(FinishDialogCommand))]
        private ClockDTO? selectedClock;

        public PlannerTemplateSelectClockViewModel(IWindowService windowService,
                                                   IDispatcherService dispatcherService,
                                                   IClocksService clocksService) : base(windowService)
        {
            this.dispatcherService = dispatcherService;
            this.clocksService = clocksService;
            _ = LoadClocks();
        }

        private async Task LoadClocks()
        {
            await Task.Run(() =>
            {
                var clocks = clocksService.GetClocks().ToList();
                Clocks.Clear();
                foreach (var clock in clocks)
                {
                    dispatcherService.InvokeOnUIThreadAsync(() =>
                    {
                         Clocks.Add(clock);
                    });
                   
                }
            });
        }

        protected override bool CanFinishDialog()
        {
            return SelectedClock != null;
        }
    }
}
