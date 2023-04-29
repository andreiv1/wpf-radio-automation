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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RA.UI.StationManagement.UserControls
{
    /// <summary>
    /// Interaction logic for LauncherComponentControl.xaml
    /// </summary>
    public partial class LauncherComponentControl : UserControl
    {
        public static readonly DependencyProperty HeaderTextProperty =
        DependencyProperty.Register("HeaderText", typeof(string), typeof(LauncherComponentControl));

        public static readonly DependencyProperty HeaderDescriptionProperty =
        DependencyProperty.Register("Description", typeof(string), typeof(LauncherComponentControl));

        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register("BackgroundColor", typeof(Brush), typeof(LauncherComponentControl));

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(LauncherComponentControl));
        public string HeaderText
        {
            get { return (string)GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }

        public string Description
        {
            get { return (string)GetValue(HeaderDescriptionProperty); }
            set { SetValue(HeaderDescriptionProperty, value); }
        }

        public Brush BackgroundColor
        {
            get { return (Brush)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public event EventHandler OpenButtonClick;

        public LauncherComponentControl()
        {
            InitializeComponent();
            OpenButton.Click += OnOpenButtonClick;
        }

        private void OnOpenButtonClick(object sender, RoutedEventArgs e)
        {
            OpenButtonClick?.Invoke(this, EventArgs.Empty);
        }
    }
}
