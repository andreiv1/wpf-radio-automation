using RA.DAL;
using RA.DTO;
using Syncfusion.UI.Xaml.TreeView.Engine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace RA.UI.StationManagement.Services.Navigation.Helpers
{
    public class CategoriesTreeMenuNavigationHelper
    {
        //private async Task LoadRootCategories(MenuItemModel menuItem)
        //{
        //    var categories = await categoriesService.GetRootCategoriesAsync();
        //    foreach (var category in categories)
        //    {
        //        if (category.Id.HasValue)
        //        {
        //            var child = new MenuItemModel
        //            {
        //                DisplayName = category.Name,
        //                HasChildNodes = await categoriesService.HasCategoryChildren(category.Id.Value),
        //                IconKey = "FolderTreeIcon",
        //                Tag = category,
        //                Type = MenuItemType.Category,
        //                NavigationCommand = new CategoryNavigationCommand(navigationService, (int)category.Id),
        //            };
        //            dispatcher.InvokeOnUIThread(() =>
        //            {
        //                menuItem.Children?.Add(child);
        //            });
        //        }
        //    }
        //}

        //private async Task LoadChildCategories(TreeViewNode parentNode)
        //{
        //    var parentCategory = parentNode.Content as MenuItemModel;
        //    if (parentCategory == null && parentCategory!.Type != MenuItemType.Category)
        //    {
        //        return;
        //    }

        //    CategoryDto? categoryDto = parentCategory.Tag as CategoryDto ?? null;
        //    if (categoryDto != null && categoryDto.Id.HasValue)
        //    {
        //        var childCategories = await categoriesService.GetChildrenCategoriesAsync(categoryDto.Id.Value);
        //        if (childCategories == null)
        //        {
        //            return;
        //        }

        //        var childItems = new ObservableCollection<MenuItemModel>();
        //        foreach (var childCategory in childCategories)
        //        {
        //            var childItem = new MenuItemModel
        //            {
        //                DisplayName = childCategory.Name,
        //                HasChildNodes = await categoriesService.HasCategoryChildren(childCategory.Id!.Value),
        //                IconKey = "FolderTreeIcon",
        //                Tag = childCategory,
        //                Type = MenuItemType.Category,
        //                NavigationCommand = new CategoryNavigationCommand(navigationService, (int)childCategory.Id),
        //            };

        //            childItem.IconKey = childItem.HasChildNodes ? "FolderTreeIcon" : "MusicFolderIcon";

        //            childItems.Add(childItem);
        //        }

        //        dispatcher.InvokeOnUIThread(() =>
        //        {
        //            parentNode.PopulateChildNodes(childItems);
        //        });
        //    }
        //}
    }
}
