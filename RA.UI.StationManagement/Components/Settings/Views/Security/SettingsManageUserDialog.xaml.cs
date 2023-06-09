using RA.UI.Core;
using RA.UI.StationManagement.Components.Settings.ViewModels.Security;

namespace RA.UI.StationManagement.Components.Settings.Views.Security
{
    public partial class SettingsManageUserDialog : RAWindow
    {
        public SettingsManageUserDialog()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            var vm = DataContext as SettingsManageUserViewModel;
            if (vm == null) return;
            vm.Password = passwordBox.Password;
        }
    }
}
