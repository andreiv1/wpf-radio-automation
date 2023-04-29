﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.DAL;
using RA.Dto;
using RA.UI.Core.Services;
using Syncfusion.UI.Xaml.TreeView.Engine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace RA.UI.StationManagement.Services
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
    }
    public partial class MediaLibraryTreeMenuService
    {
        private readonly IDispatcherService dispatcher;
        private readonly ICategoriesService categoriesService;
        private readonly ITagsService tagsService;

        public ObservableCollection<MenuItemModel> MenuItems { get; set; } = new();

        public MediaLibraryTreeMenuService(IDispatcherService dispatcher, ICategoriesService categoriesService, 
            ITagsService tagsService)
        {
            this.dispatcher = dispatcher;
            this.categoriesService = categoriesService;
            this.tagsService = tagsService;
            MenuItems = GetMenuItems();
        }

        [RelayCommand(CanExecute = nameof(CanExecuteOnDemandLoading))]
        private void ExecuteOnDemandLoading(object obj)
        {
            var node = obj as TreeViewNode;
            node!.ShowExpanderAnimation = true;
            var menuItem = node.Content as MenuItemModel;
            dispatcher.InvokeOnUIThread(new Action(
                async () =>
                {
                    await Task.Delay(300);
                    switch (menuItem.Type)
                    {
                        case MenuItemType.Other:
                            break;
                        case MenuItemType.Category:
                            await Task.Run(() => LoadChildCategories(node));
                            break;
                        case MenuItemType.Tag:
                            await Task.Run(() => LoadTags(node));
                            break;
                    }
                    node!.ShowExpanderAnimation = false;
                    node!.IsExpanded = true;
                }));
            
        }

        private bool CanExecuteOnDemandLoading(object sender)
        {
            var hasChildNodes = ((sender as TreeViewNode)!.Content as MenuItemModel)!.HasChildNodes;
            return hasChildNodes;
        }

        private ObservableCollection<MenuItemModel> GetMenuItems()
        {
            ObservableCollection<MenuItemModel> menuItems = new();

            var allItems = new MenuItemModel { DisplayName = "All Items", HasChildNodes = false, IconKey = "MusicalNotesIcon" };
            menuItems.Add(allItems);
            var artists = new MenuItemModel { DisplayName = "Artists", HasChildNodes = false, IconKey = "MusicBandIcon", };
            menuItems.Add(artists);

            var categories = new MenuItemModel { DisplayName = "Categories", HasChildNodes = true, IconKey = "FolderTreeIcon", Type = MenuItemType.Category };
            menuItems.Add(categories);

            var tags = new MenuItemModel { DisplayName = "Tags", HasChildNodes = true, IconKey = "TagsIcon", Type = MenuItemType.Tag };
            menuItems.Add(tags);

            tags.Children?.Add(new MenuItemModel { DisplayName = "Loading...", HasChildNodes = false, Type = MenuItemType.Tag });

            Task.Run(() => LoadRootCategories(categories));
            return menuItems;
        }

        private async Task LoadRootCategories(MenuItemModel menuItem)
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
                    };
                    dispatcher.InvokeOnUIThread(() =>
                    {
                        menuItem.Children?.Add(child);
                    });
                }
            }
        }

        private async Task LoadChildCategories(TreeViewNode parentNode)
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
                foreach (var childCategory in childCategories) {
                    var childItem = new MenuItemModel
                    {
                        DisplayName = childCategory.Name,
                        HasChildNodes = await categoriesService.HasCategoryChildren(childCategory.Id!.Value),
                        IconKey = "FolderTreeIcon",
                        Tag = childCategory,
                        Type = MenuItemType.Category,
                    };

                    childItem.IconKey = childItem.HasChildNodes ? "FolderTreeIcon" : "MusicFolderIcon";

                    childItems.Add(childItem);
                }

                dispatcher.InvokeOnUIThread(() =>
                {
                    parentNode.PopulateChildNodes(childItems);
                });
            }
        }

        private async Task LoadTags(TreeViewNode tagsParentNode)
        {
            var parentCategory = tagsParentNode.Content as MenuItemModel;
            if (parentCategory == null && parentCategory!.Type != MenuItemType.Tag)
            {
                return;
            }

            var tags = await tagsService.GetTagCategoriesAsync();
            var childItems = new ObservableCollection<MenuItemModel>();
            foreach(var tag in tags)
            {
                var childTag = new MenuItemModel
                {
                    DisplayName = tag.Name,
                    HasChildNodes = false,
                    IconKey = "TagsIcon",
                };

                childItems.Add(childTag);
            }

            dispatcher.InvokeOnUIThread(() =>
            {
                tagsParentNode.PopulateChildNodes(childItems);
            });
        }
    }
}
