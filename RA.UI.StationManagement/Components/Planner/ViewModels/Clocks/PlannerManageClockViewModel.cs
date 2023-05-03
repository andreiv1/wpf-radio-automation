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

namespace RA.UI.StationManagement.Components.Planner.ViewModels.Clocks
{
    public partial class PlannerManageClockViewModel : DialogViewModelBase
    {
        [ObservableProperty]
        private ClockDto clockDto = new();
        private bool duplicate = false;
        private readonly IClocksService clocksService;

        public PlannerManageClockViewModel(IWindowService windowService, IClocksService clocksService) 
            : base(windowService)
        {
            DialogName = "Add new clock";
            this.clocksService = clocksService;
        }

        public PlannerManageClockViewModel(IWindowService windowService, IClocksService clocksService, int clockId, bool duplicate = false) 
            : base(windowService)
        {
            DialogName = "Edit clock";
            this.clocksService = clocksService;
            this.clockDto.Id = clockId;
            this.duplicate = duplicate;
            _ = LoadClock();
        }

        #region Data fetching
        private async Task LoadClock()
        {
            if (ClockDto?.Id != null)
            {
                ClockDto = await clocksService.GetClock(ClockDto.Id.GetValueOrDefault());

                if (duplicate)
                {
                    ClockDto.Id = null;
                    ClockDto.Name += " (COPY)";
                }

            }
        }
        #endregion

        #region Commands
        //[RelayCommand] in base class
        protected override void FinishDialog()
        {
            base.FinishDialog();
        }


        protected override bool CanFinishDialog()
        {
            return false;
        }

        #endregion
    }
}
