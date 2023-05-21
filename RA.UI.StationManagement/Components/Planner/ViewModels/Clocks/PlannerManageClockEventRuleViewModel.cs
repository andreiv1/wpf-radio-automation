using RA.Database.Models.Enums;
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
        public PlannerManageClockEventRuleViewModel(IWindowService windowService) : base(windowService)
        {
        }

        protected override bool CanFinishDialog()
        {
            return false;
        }
    }
}
