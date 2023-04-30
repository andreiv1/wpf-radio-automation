using Syncfusion.SfSkinManager;
using Syncfusion.Themes.Windows11Dark.WPF;
using Syncfusion.Themes.Windows11Light.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RA.UI.Core.Themes
{
    public enum ThemeType
    {
        Light,
        Dark
    }
    public static class ThemeManager
    {
        private static ThemeType type;

        private static Theme? theme;

        public static void SetTheme(ThemeType type)
        {
            switch (type)
            {
                case ThemeType.Light:
                    Windows11LightThemeSettings windows11LightThemeSettings = new Windows11LightThemeSettings();
                    SfSkinManager.RegisterThemeSettings("Windows11Light", windows11LightThemeSettings);
                    theme = new Theme("Windows11Light");
                    break;
                case ThemeType.Dark:
                    Windows11DarkThemeSettings windows11DarkThemeSettings = new Windows11DarkThemeSettings();
                    SfSkinManager.RegisterThemeSettings("Windows11Dark", windows11DarkThemeSettings);
                    theme = new Theme("Windows11Dark");
                    break;
            }
        }

        public static void ApplyTheme(DependencyObject view)
        {
            SfSkinManager.SetTheme(view, theme);
        }
    }
}
