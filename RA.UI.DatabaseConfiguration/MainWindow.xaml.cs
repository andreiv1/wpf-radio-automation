using Syncfusion.Windows.Shared;
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

namespace RA.UI.DatabaseConfiguration
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ChromelessWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadPassword();
        }

        private void LoadPassword()
        {
            var vm = DataContext as MainWindowViewModel;
            if (vm is not null)
            {
                dbPassword.Password = vm.DatabaseCredentials.DatabasePassword;
            }
        }

        private void dbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainWindowViewModel;
            if(vm is not null)
            {
                vm.NewPassword = dbPassword.Password;
            }
        }
    }
}
