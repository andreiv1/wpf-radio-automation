using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public partial class Soundwave : UserControl
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
            webView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            webView.CoreWebView2.Settings.AreBrowserAcceleratorKeysEnabled = false;
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

    }
}
