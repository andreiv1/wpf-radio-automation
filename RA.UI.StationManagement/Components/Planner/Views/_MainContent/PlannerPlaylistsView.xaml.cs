using RA.UI.Core;
using RA.UI.StationManagement.Components.Planner.ViewModels.MainContent;
using System;
using System.Windows;

namespace RA.UI.StationManagement.Components.Planner.Views.MainContent
{
    public partial class PlannerPlaylistsView : RAUserControl, IDisposable
    {
        public PlannerPlaylistsView()
        {
            InitializeComponent();
            playlistItems.RowDragDropController.Drop += RowDragDropController_Drop;
        }

        private void RowDragDropController_Drop(object? sender, Syncfusion.UI.Xaml.Grid.GridRowDropEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void Expander_Expanded_1(object sender, RoutedEventArgs e)
        {
            if (Expander2 is not null)
            {
                Expander2.IsExpanded = !Expander1.IsExpanded;
                ExpanderFirstRow.Height = Expander1.IsExpanded ? new GridLength(1, GridUnitType.Star) : new GridLength(1, GridUnitType.Auto);
                ExpanderSecondRow.Height = Expander2.IsExpanded ? new GridLength(1, GridUnitType.Star) : new GridLength(1, GridUnitType.Auto);
            }
        }

        private void Expander_Expanded_2(object sender, RoutedEventArgs e)
        {
            if (Expander1 is not null)
            {
                Expander1.IsExpanded = !Expander2.IsExpanded;
                ExpanderFirstRow.Height = Expander1.IsExpanded ? new GridLength(1, GridUnitType.Star) : new GridLength(1, GridUnitType.Auto);
                ExpanderSecondRow.Height = Expander2.IsExpanded ? new GridLength(1, GridUnitType.Star) : new GridLength(1, GridUnitType.Auto);
            }
        }

        private void Expander_Collapsed_1(object sender, RoutedEventArgs e)
        {
            if (Expander2 is not null && !Expander2.IsExpanded)
            {
                Expander2.IsExpanded = true;
            }
        }

        private void Expander_Collapsed_2(object sender, RoutedEventArgs e)
        {
            if (Expander1 is not null && !Expander1.IsExpanded)
            {
                Expander1.IsExpanded = true;
            }
        }

        protected override void ToDispose()
        {
            playlistItems.RowDragDropController.Drop -= RowDragDropController_Drop;
        }



    }
}

