using System.Reflection;
using System.Windows.Controls;

namespace RA.UI.StationManagement.Components.Settings.Views.MainContent
{
    /// <summary>
    /// Interaction logic for SettingsAboutView.xaml
    /// </summary>
    public partial class SettingsAboutView : UserControl
    {
        public SettingsAboutView()
        {
            InitializeComponent();
            versionTextblock.Text = $"Version {SplashScreenWindow.AssemblyVersion}";
        }
    }
}
