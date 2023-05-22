using RA.UI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Application = System.Windows.Application;
using MessageBoxOptions = System.Windows.MessageBoxOptions;

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
    }
}
