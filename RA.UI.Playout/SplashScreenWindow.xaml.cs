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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RA.UI.Playout
{
    /// <summary>
    /// Interaction logic for SplashScreenWindow.xaml
    /// </summary>
    public partial class SplashScreenWindow : Window
    {
        public SplashScreenWindow()
        {
            InitializeComponent();
        }

        private void progressBar_Loaded(object sender, RoutedEventArgs e)
        {
            var progressBar = sender as ProgressBar;
            var progressBarTemplate = progressBar.Template;

            var partIndicator = progressBarTemplate.FindName("PART_Indicator", progressBar) as Grid;
            var partTrack = progressBarTemplate.FindName("PART_Track", progressBar) as Grid;

            var storyboard = new Storyboard();
            var doubleAnimation = new DoubleAnimation
            {
                From = -400,
                To = partTrack.ActualWidth,
                Duration = TimeSpan.FromSeconds(2),
                RepeatBehavior = RepeatBehavior.Forever
            };

            Storyboard.SetTarget(doubleAnimation, partIndicator);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
            storyboard.Children.Add(doubleAnimation);
            storyboard.Begin();
        }
    }
}
