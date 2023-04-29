using RA.Logic;
using RA.UI.StationManagement.Components.Planner.ViewModels.MainContent.Models;
using RA.UI.StationManagement.Components.Planner.ViewModels.MainContent;
using Syncfusion.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RA.UI.StationManagement.Components.Planner.View.MainContent
{
    /// <summary>
    /// Interaction logic for PlannerDayTemplatesView.xaml
    /// </summary>
    public partial class PlannerDayTemplatesView : UserControl
    {
        public PlannerDayTemplatesView()
        {
            InitializeComponent();
            DebugHelper.WriteLine(this, $"SCHEDULER ITEMS: this.selectedTemplateScheduler.ItemsSource");
            DataContextChanged += PlannerDayTemplatesView_DataContextChanged;
        }

        private void PlannerDayTemplatesView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            DebugHelper.WriteLine(this, $"Data context changed");
            var dataContext = DataContext as PlannerDayTemplatesViewModel;
            selectedTemplateScheduler.ItemsSource = dataContext?.ClocksForSelectedTemplate;
        }

    }
}
