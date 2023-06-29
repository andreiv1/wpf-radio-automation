using RA.UI.Core.Factories;
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
        public ObservableCollection<TabViewModel> TabViewModels { get; private set; }
        public PlannerScheduleViewModel(IViewModelFactory viewModelFactory)
        {
            TabViewModels = new ObservableCollection<TabViewModel>()
            {
                new TabViewModel(viewModelFactory, "Calendar", (ImageSource)Application.Current.Resources["CalendarIcon"], typeof(PlannerScheduleCalendarViewModel)),
                new TabViewModel(viewModelFactory, "Default schedules", (ImageSource)Application.Current.Resources["DefaultScheduleIcon"], typeof(PlannerDefaultScheduleViewModel)),
            };
        }
        

    }
}
