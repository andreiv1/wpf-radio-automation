using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.Database.Models;
using RA.Database;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RA.DAL;
using RA.DTO;
using System.Configuration;
using RA.UI.StationManagement.Components.Planner.ViewModels.MainContent.Models;
using System.ComponentModel;

namespace RA.UI.StationManagement.Components.Planner.ViewModels.Clocks
{
    public partial class PlannerManageClockViewModel : DialogViewModelBase
    {
        [ObservableProperty]
        private ClockModel managedClock = new();

        private bool duplicate = false;
        private readonly IClocksService clocksService;

       
        public PlannerManageClockViewModel(IWindowService windowService,
                                           IClocksService clocksService) 
            : base(windowService)
        {
            DialogName = "Add new clock";
            this.clocksService = clocksService;
            managedClock.ErrorsChanged += ManagedClock_ErrorsChanged;
            ManagedClock.Validate();
        }

        public PlannerManageClockViewModel(IWindowService windowService,
                                           IClocksService clocksService,
                                           int clockId,
                                           bool duplicate = false) 
            : base(windowService)
        {
            DialogName = "Edit clock";
            this.clocksService = clocksService;
            ManagedClock.Id = clockId;
            this.duplicate = duplicate;
            managedClock.ErrorsChanged += ManagedClock_ErrorsChanged;
            _ = LoadClock();

        }
       

        private void ManagedClock_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
        {
            if (e.PropertyName == "Name")
            {
                FinishDialogCommand.NotifyCanExecuteChanged();
            }
        }
        

        //Data fetching
        private async Task LoadClock()
        {
            if (ManagedClock?.Id != null)
            {
                var dto = await clocksService.GetClock(ManagedClock.Id);
                ManagedClock = ClockModel.FromDto(dto);
                if (duplicate)
                {
                    ManagedClock.Id = 0;
                    ManagedClock.Name += " (COPY)";
                }

            }
        }


        #region Commands
        //[RelayCommand] in base class
        protected override void FinishDialog()
        {
            base.FinishDialog();
        }


        protected override bool CanFinishDialog()
        {
            return !ManagedClock.HasErrors;
        }
        #endregion


        public override void Dispose()
        {
            ManagedClock.ErrorsChanged -= ManagedClock_ErrorsChanged;
            base.Dispose();
        }

    }
}
