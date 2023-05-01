using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace RA.UI.StationManagement.Services.Navigation
{
    public enum MenuItemType
    {
        Other,
        Category,
        Tag,
    }
    public partial class MenuItemModel : ObservableObject
    {
        [ObservableProperty]
        private String displayName = "Unknown";
        [ObservableProperty]
        private bool hasChildNodes = false;
        partial void OnHasChildNodesChanged(bool oldValue, bool newValue)
        {
            if (newValue)
            {
                Children = new();
            }
            else
            {
                Children?.Clear();
            }
        }
        public ObservableCollection<MenuItemModel>? Children { get; set; }
        public object? Tag { get; set; }

        private BitmapImage? icon;
        public BitmapImage? Icon
        {
            get
            {
                if (icon == null && !string.IsNullOrEmpty(IconKey))
                {
                    var resourceDictionary = Application.Current.Resources;
                    if (resourceDictionary.Contains(IconKey))
                    {
                        icon = resourceDictionary[IconKey] as BitmapImage;
                    }
                }
                return icon;
            }
        }
        public string? IconKey { get; set; }
        public MenuItemType Type { get; set; } = MenuItemType.Other;

        public ICommand? NavigationCommand { get; set; }
    }
}
