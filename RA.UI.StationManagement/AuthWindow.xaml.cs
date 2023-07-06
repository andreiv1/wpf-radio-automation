using RA.UI.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace RA.UI.StationManagement
{
    /// <summary>
    /// Interaction logic for AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : RAWindow
    {
        public AuthWindow()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as AuthViewModel;
            if (vm == null) return;
            var passwordBox = sender as PasswordBox;
            if(passwordBox == null) return; 
            vm.Password = passwordBox.Password;
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.System && e.SystemKey == Key.F4)
            {
                Application.Current.Shutdown();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            var vm = DataContext as AuthViewModel;
            if (vm?.UserStore.LoggedUser != null)
            {
                
            } else
            {
                e.Cancel = true;
                Application.Current.Shutdown();
            }
            
        }
    }
}
