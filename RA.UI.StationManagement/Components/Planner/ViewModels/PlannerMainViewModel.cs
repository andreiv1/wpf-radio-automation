using Microsoft.Extensions.DependencyInjection;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.Planner.ViewModels.MainContent;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace RA.UI.StationManagement.Components.Planner.ViewModels
{
    public partial class PlannerMainViewModel : ViewModelBase
    {
        public ObservableCollection<TabViewModel> TabViewModels { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public PlannerMainViewModel()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            InitializeTabViewModels();
        }

        private void InitializeTabViewModels()
        {
            var services = App.AppHost!.Services;
            TabViewModels = new ObservableCollection<TabViewModel>
            {
                new TabViewModel(services, typeof(PlannerClocksViewModel))
                {
                    TabHeaderName = "Clocks",
                    TabIcon = (ImageSource)Application.Current.Resources["ClockIcon"]

                },
                new TabViewModel(services, typeof(PlannerDayTemplatesViewModel))
                {
                    TabHeaderName = "Day Templates",
                    TabIcon = (ImageSource) Application.Current.Resources["TemplateIcon"]
                },
                new TabViewModel(services, typeof(PlannerScheduleViewModel))
                {
                    TabHeaderName = "Schedule",
                    TabIcon = (ImageSource) Application.Current.Resources["ScheduleIcon"]
                },
                new TabViewModel(services, typeof(PlannerPlaylistsViewModel))
                {
                    TabHeaderName = "Playlists",
                    TabIcon = (ImageSource) Application.Current.Resources["PlaylistIcon"]
                },

            };
        }
    }
}
