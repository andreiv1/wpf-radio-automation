using RA.UI.Core;
using RA.UI.Core.Shared;
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

namespace RA.UI.Playout
{
    /// <summary>
    /// Interaction logic for DatabaseSetupWindow.xaml
    /// </summary>
    public partial class DatabaseSetupWindow : RAWindow
    {
        public DatabaseSetupWindow()
        {
            InitializeComponent();
        }

        private void dbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var vm = (DatabaseSetupViewModel)DataContext;
            vm.DbPassword = dbPassword.Password;
        }
    }
}
