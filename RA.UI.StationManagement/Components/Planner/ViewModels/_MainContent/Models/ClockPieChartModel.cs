using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.Planner.ViewModels.MainContent.Models
{
    public partial class ClockPieChartModel : ObservableObject
    {
        [ObservableProperty]
        public String itemName;

        [ObservableProperty]
        public int totalSeconds;

        public ClockPieChartModel(string itemName, int totalSeconds)
        {
            this.itemName = itemName;
            this.totalSeconds = totalSeconds;
        }
    }
}
