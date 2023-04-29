using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.Planner.View.Schedule;
using RA.UI.StationManagement.Components.Planner.ViewModels.Schedule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace RA.UI.StationManagement.Components.Planner.ViewModels.MainContent
{
    public partial class PlannerScheduleViewModel : ViewModelBase
    {
        public ObservableCollection<TabViewModel> TabViewModels { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public PlannerScheduleViewModel()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            InitializeTabViewModels();
        }

        private void InitializeTabViewModels()
        {
            var services = App.AppHost!.Services;
            TabViewModels = new ObservableCollection<TabViewModel>
            {
                new TabViewModel(services, typeof(PlannerScheduleCalendarViewModel))
                {
                    TabHeaderName = "Calendar",
                    TabIcon = (ImageSource)Application.Current.Resources["CalendarIcon"]

                },
                new TabViewModel(services, typeof(PlannerDefaultScheduleViewModel))
                {
                    TabHeaderName = "Default schedules",
                    TabIcon = (ImageSource) Application.Current.Resources["DefaultScheduleIcon"]
                },

            };
        }
    }
}
