using RA.UI.Core.ViewModels;
using Syncfusion.SfSkinManager;
using Syncfusion.Themes.Windows11Light.WPF;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RA.UI.Core
{
    public partial class RAWindow : ChromelessWindow
    {
        private bool disposedValue;

        public static Windows11LightThemeSettings ThemeSettings { get; set; } = new Windows11LightThemeSettings();

        private static Thickness thickness = new Thickness(0);
        private static CornerRadius cornerRadius = new CornerRadius(0);

        static RAWindow()
        {
            Windows11LightThemeSettings windows11LightThemeSettings = new Windows11LightThemeSettings();
            SfSkinManager.RegisterThemeSettings("Windows11Light", windows11LightThemeSettings);
        }
        public RAWindow() : base()
        {
            SfSkinManager.SetTheme(this, new Theme("Windows11Light"));
            this.ResizeBorderThickness = thickness;
            this.CornerRadius = cornerRadius;
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    var viewModel = DataContext as ViewModelBase;
                    viewModel?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~RAWindow()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
