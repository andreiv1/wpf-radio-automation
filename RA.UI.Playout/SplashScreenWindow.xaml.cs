using RA.UI.Core;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace RA.UI.Playout
{
    public partial class SplashScreenWindow : RAWindow
    {
        private static string assemblyVersion;

        public static string AssemblyVersion => assemblyVersion;
        public SplashScreenWindow()
        {
            InitializeComponent();
            assemblyVersion = Assembly.GetCallingAssembly().GetName().Version.ToString(2);
            appVersion.Text = $"Version {assemblyVersion}";
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
