using RA.DAL;
using RA.DTO;
using RA.UI.Core.Services;
using Syncfusion.UI.Xaml.TreeView.Engine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace RA.UI.StationManagement.Services.Navigation
{
    public interface ICategoriesTreeMenuService
    {
        Task LoadChildCategories(TreeViewNode parentNode);
        Task LoadRootCategories(MenuItemModel menuItem);
    }

    public class CategoriesTreeMenuService : ICategoriesTreeMenuService
    {
        private readonly IDispatcherService dispatcherService;
        private readonly ICategoriesService categoriesService;

        public CategoriesTreeMenuService(IDispatcherService dispatcherService,
            ICategoriesService categoriesService)
        {
            this.dispatcherService = dispatcherService;
            this.categoriesService = categoriesService;
        }

        public async Task LoadRootCategories(MenuItemModel menuItem)
        {
            var categories = await categoriesService.GetRootCategoriesAsync();
            foreach (var category in categories)
            {
                if (category.Id.HasValue)
                {
                    var child = new MenuItemModel
                    {
                        DisplayName = category.Name,
                        HasChildNodes = await categoriesService.HasCategoryChildren(category.Id.Value),
                        IconKey = "FolderTreeIcon",
                        Tag = category,
                        Type = MenuItemType.Category,
                        //NavigationCommand = new CategoryNavigationCommand(navigationService, (int)category.Id),
                    };
                    dispatcherService.InvokeOnUIThread(() =>
                    {
                        menuItem.Children?.Add(child);
                    });
                }
            }
        }

        public async Task LoadChildCategories(TreeViewNode parentNode)
        {
            var parentCategory = parentNode.Content as MenuItemModel;
            if (parentCategory == null && parentCategory!.Type != MenuItemType.Category)
            {
                return;
            }

            CategoryDto? categoryDto = parentCategory.Tag as CategoryDto ?? null;
            if (categoryDto != null && categoryDto.Id.HasValue)
            {
                var childCategories = await categoriesService.GetChildrenCategoriesAsync(categoryDto.Id.Value);
                if (childCategories == null)
                {
                    return;
                }

                var childItems = new ObservableCollection<MenuItemModel>();
                foreach (var childCategory in childCategories)
                {
                    var childItem = new MenuItemModel
                    {
                        DisplayName = childCategory.Name,
                        HasChildNodes = await categoriesService.HasCategoryChildren(childCategory.Id!.Value),
                        IconKey = "FolderTreeIcon",
                        Tag = childCategory,
                        Type = MenuItemType.Category,
                        //NavigationCommand = new CategoryNavigationCommand(navigationService, (int)childCategory.Id),
                    };

                    childItem.IconKey = childItem.HasChildNodes ? "FolderTreeIcon" : "MusicFolderIcon";

                    childItems.Add(childItem);
                }

                dispatcherService.InvokeOnUIThread(() =>
                {
                    parentNode.PopulateChildNodes(childItems);
                });
            }
        }
    }
}
