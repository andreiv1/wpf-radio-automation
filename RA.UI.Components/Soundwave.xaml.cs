using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
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

namespace RA.UI.Components
{
    /// <summary>
    /// Interaction logic for Soundwave.xaml
    /// </summary>
    public partial class Soundwave : UserControl, IDisposable
    {
        public Soundwave()
        {
            InitializeComponent();

            _ = InitializeAsync();

            webView.Source = new Uri(string.Format("file:///{0}/Soundwave/html/index.html", Directory.GetCurrentDirectory()));

            Loaded += (s, e) =>
            {
                Window.GetWindow(this)
                    .Closing += (s1, e1) => CloseWebView();
            };
        }

        private async Task InitializeAsync()
        {
            var options = new CoreWebView2EnvironmentOptions("--disable-web-security");
            var environment = await CoreWebView2Environment.CreateAsync(null, null, options);
            await webView.EnsureCoreWebView2Async(environment);
            //webView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            //webView.CoreWebView2.Settings.AreBrowserAcceleratorKeysEnabled = false;
        }

        private void CloseWebView()
        {
            webView.Dispose();
        }

        private void WebView_NavigationStarting(object? sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs e)
        {
            //webView.CoreWebView2.OpenDevToolsWindow();
        }

        public static readonly DependencyProperty FilePathProperty =
        DependencyProperty.Register("FilePath", typeof(string), typeof(Soundwave),
            new PropertyMetadata(null, OnFilePathChanged));

        public static readonly DependencyProperty StartCueProperty =
            DependencyProperty.Register("StartCue", typeof(double), typeof(Soundwave),
                new PropertyMetadata(0.0, OnStartCueChanged));


        public static readonly DependencyProperty NextCueProperty =
            DependencyProperty.Register("NextCue", typeof(double), typeof(Soundwave),
                new PropertyMetadata(0.0, OnNextCueChanged));

        public static readonly DependencyProperty EndCueProperty =
            DependencyProperty.Register("EndCue", typeof(double), typeof(Soundwave),
                new PropertyMetadata(0.0, OnEndCueChanged));

        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(double), typeof(Soundwave),
                new PropertyMetadata(0.0, OnDurationChanged));

        public double Duration
        {
            get { return (double)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        public double StartCue
        {
            get { return (double)GetValue(StartCueProperty); }
            set { SetValue(StartCueProperty, value); }
        }

        public double NextCue
        {
            get { return (double)GetValue(NextCueProperty); }
            set { SetValue(NextCueProperty, value);}
        }

        public double EndCue
        {
            get { return (double)GetValue(EndCueProperty); }
            set { SetValue(EndCueProperty, value); }
        }

        private static void OnDurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Soundwave)d;
        }

        private static void OnStartCueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Soundwave)d;
        }
        private static void OnNextCueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Soundwave)d;
        }

        private static void OnEndCueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Soundwave)d;
        }

        public string FilePath
        {
            get { return (string)GetValue(FilePathProperty); }
            set { SetValue(FilePathProperty, value); }
        }

        public async void ExecuteJs(String s)
        {
            Debug.WriteLine("Executing " + s);
            await webView.ExecuteScriptAsync($"console.log('{s}')");
            await webView.ExecuteScriptAsync(s);
        }

        private static void OnFilePathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Soundwave)d;     
            var path = control.FilePath.Replace(@"\",@"\\")
                .Replace(@"'",@"\'");
            Debug.WriteLine("[SoundwaveComponent] File path changed to : " + control.FilePath);
           
            var jsFuncCallStr = $"loadFile('{path}')";
            control.webView.NavigationCompleted += (s, e) =>
            {
                control.ExecuteJs("console.log('Test path')");
                control.ExecuteJs(jsFuncCallStr);
                
                control.ExecuteJs($"addStartCue({control.StartCue})");
                control.ExecuteJs($"addEndCue({control.EndCue})");
                control.ExecuteJs($"addNextCue({control.NextCue})");
            };
        }

        public void Play()
        {
            ExecuteJs("play()");
        }

        public void Pause()
        {
            ExecuteJs("pause()");
        }

        public async Task<List<double>> GetMarkers()
        {
            var markers = new List<double>();
            string markersString = await webView.ExecuteScriptAsync("getMarkersTime()");
            markersString = markersString.Replace("\\", "").Replace("\"", "").Replace("\"","");
            Debug.WriteLine($"Markers: {markersString}");
            string[] tokens = markersString.Split(";");
            foreach(var token in tokens)
            {
                var split = token.Split('=');
                markers.Add(Convert.ToDouble(split[1], CultureInfo.InvariantCulture));

                
            }
            return markers;
        }

        public void Dispose()
        {
            Loaded -= null;
        }
    }
}
