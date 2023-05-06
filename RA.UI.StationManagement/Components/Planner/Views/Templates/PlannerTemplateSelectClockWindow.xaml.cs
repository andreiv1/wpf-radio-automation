using RA.DTO;
using RA.Logic;
using RA.UI.Core;
using Syncfusion.UI.Xaml.Grid;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace RA.UI.StationManagement.Components.Planner.Views.Templates
{
    public partial class PlannerTemplateSelectClockWindow : RAWindow
    {
        public PlannerTemplateSelectClockWindow()
        {
            InitializeComponent();
            sfDataGrid.RowDragDropController.DragStart += RowDragDropController_DragStart;
        }

        private void RowDragDropController_DragStart(object? sender, GridRowDragStartEventArgs e)
        {
            var draggingClock = e.DraggingRecords[0] as ClockDTO;
            if (draggingClock == null) return;
            DragDrop.DoDragDrop(sfDataGrid, draggingClock, DragDropEffects.Move);
        }
    }
}
