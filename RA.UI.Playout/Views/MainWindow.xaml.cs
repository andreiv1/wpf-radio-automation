using RA.UI.Core;
using System.Windows;
using Application = System.Windows.Application;

namespace RA.UI.Playout.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RAWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RAWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("Are you sure you must exit the playout system?", "Warning", MessageBoxButton.OKCancel, MessageBoxImage.None);
            if(result == System.Windows.MessageBoxResult.OK)
            {
                Application.Current.Shutdown();
            } else
            {
                e.Cancel = true;
            }
        }

        private void ButtonAdv_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void playlistMode_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
