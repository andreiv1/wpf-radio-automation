using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace RA.UI.Core.Models.UI
{
    public class NavigationTreeItem
    {
        public String? DisplayName { get; set; }
        public String? InternalName { get; set; }
        public ICommand? NavigationCommand { get; set; }

        public object? Tag { get; set; }

        public bool HasChildren { get; set; } = false;
        public bool IsRoot { get; set; } = false;
        public ObservableCollection<NavigationTreeItem>? Children { get; set; } = null;

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
    }
}
