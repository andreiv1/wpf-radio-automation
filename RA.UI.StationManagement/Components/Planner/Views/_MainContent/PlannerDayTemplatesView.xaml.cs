using RA.DTO;
using RA.Logic;
using RA.UI.Core;
using RA.UI.StationManagement.Components.Planner.ViewModels.MainContent;
using RA.UI.StationManagement.Components.Planner.ViewModels.MainContent.Models;
using Syncfusion.UI.Xaml.Scheduler;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RA.UI.StationManagement.Components.Planner.Views.MainContent
{
    public partial class PlannerDayTemplatesView : RAUserControl
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


        private DateTime RoundDate(DateTime date)
        {
            if(date.Minute < 30)
            {
                date = date.AddMinutes(-date.Minute);
            } else
            {
                date = date.AddMinutes(60 - date.Minute);
            }
            return date;
        }
        private async void selectedTemplateScheduler_AppointmentResizing(object sender, AppointmentResizingEventArgs e)
        {
            

            if (e.Action == ResizeAction.Committing)
            {
                var newStart = RoundDate(e.StartTime);
                var newEnd = RoundDate(e.EndTime);
                DebugHelper.WriteLine(this, $"Resized to: {newStart} {newEnd}");
                var vm = DataContext as PlannerDayTemplatesViewModel;
                var clockModel = e.Appointment.Data as TemplateClockItemModel;
                if (vm == null || clockModel == null) return;
                await vm.UpdateClockToTemplate(clockModel.ClockId, clockModel.StartTime, newStart, newEnd);
            }
        }

      
    }
}
