using RA.UI.Core.Factories;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.Planner.ViewModels.Schedule;
using System.Collections.ObjectModel;
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
                new TabViewModel(
                    viewModelFactory, "Calendar", 
                    (ImageSource)Application.Current.Resources["CalendarIcon"], 
                    viewModelType: typeof(PlannerScheduleCalendarViewModel)),
                new TabViewModel(
                    viewModelFactory, "Default schedules", 
                    (ImageSource)Application.Current.Resources["DefaultScheduleIcon"], 
                    viewModelType: typeof(PlannerDefaultScheduleViewModel)),
            };
        }
        

    }
}
