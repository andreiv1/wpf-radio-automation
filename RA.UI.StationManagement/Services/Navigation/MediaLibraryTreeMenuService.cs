using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.DAL;
using RA.DTO;
using RA.UI.Core.Services;
using RA.UI.Core.Services.Interfaces;
using RA.UI.StationManagement.Components.MediaLibrary.ViewModels;
using RA.UI.StationManagement.Components.MediaLibrary.ViewModels.MainContent;
using Syncfusion.UI.Xaml.TreeView.Engine;
using System;
using System.Collections.ObjectModel;
using System.Linq;
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

        public ICommand? NavigationCommand { get; set; }
    }
    public class CategoryNavigationCommand : IRelayCommand
    {
        private readonly INavigationService<MediaLibraryMainViewModel> navigationService;
        private readonly int categoryId;

        public event EventHandler? CanExecuteChanged;

        public CategoryNavigationCommand(INavigationService<MediaLibraryMainViewModel> navigationService, 
            int categoryId)
        {
            this.navigationService = navigationService;
            this.categoryId = categoryId;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            navigationService.NavigateTo<CategoryItemsViewModel>(categoryId);
        }

        public void NotifyCanExecuteChanged()
        {

        }
    }
    public partial class MediaLibraryTreeMenuService : ObservableObject
    {
        private readonly IDispatcherService dispatcher;
        private readonly ICategoriesService categoriesService;
        private readonly ITagsService tagsService;
        private readonly INavigationService<MediaLibraryMainViewModel> navigationService;

        public ObservableCollection<MenuItemModel> MenuItems { get; private set; } = new();
        [ObservableProperty]
        private MenuItemModel? selectedItem;

        partial void OnSelectedItemChanged(MenuItemModel? value)
        {
            value?.NavigationCommand?.Execute(null);
        }

        public MediaLibraryTreeMenuService(IDispatcherService dispatcher,
                                           ICategoriesService categoriesService,
                                           ITagsService tagsService,
                                           INavigationService<MediaLibraryMainViewModel> navigationService)
        {
            this.dispatcher = dispatcher;
            this.categoriesService = categoriesService;
            this.tagsService = tagsService;
            this.navigationService = navigationService;

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
                    switch (menuItem?.Type)
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

            var allItems = new MenuItemModel
            {
                DisplayName = "All Items",
                HasChildNodes = false,
                IconKey = "MusicalNotesIcon",
                NavigationCommand = new RelayCommand(() =>
                {
                    navigationService.NavigateTo<AllMediaItemsViewModel>();
                })
            };
            menuItems.Add(allItems);
            var artists = new MenuItemModel
            {
                DisplayName = "Artists",
                HasChildNodes = false,
                IconKey = "MusicBandIcon",
                NavigationCommand = new RelayCommand(() =>
                {
                    navigationService.NavigateTo<ArtistsViewModel>();
                })
            };
            menuItems.Add(artists);

            var categories = new MenuItemModel
            {
                DisplayName = "Categories",
                HasChildNodes = true,
                IconKey = "FolderTreeIcon",
                Type = MenuItemType.Category,
                NavigationCommand = new RelayCommand(() =>
                {
                    navigationService.NavigateTo<CategoriesViewModel>();
                })
            };
            menuItems.Add(categories);

            var tags = new MenuItemModel { DisplayName = "Tags", HasChildNodes = true, IconKey = "TagsIcon", Type = MenuItemType.Tag,
                NavigationCommand = new RelayCommand(() =>
                {
                    navigationService.NavigateTo<TagsViewModel>();
                })};
            menuItems.Add(tags);

            tags.Children?.Add(new MenuItemModel { DisplayName = "Loading...", HasChildNodes = false, Type = MenuItemType.Tag });

            Task.Run(() => LoadRootCategories(categories));
            return menuItems;
        }

        public async Task ReloadCategories()
        {
            MenuItemModel? item = MenuItems.Where(x => x.DisplayName == "Categories").FirstOrDefault();
            if (item == null) return;
            item.Children?.Clear();
            await LoadRootCategories(item);
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
                        NavigationCommand = new CategoryNavigationCommand(navigationService, (int)category.Id),
                    };
                    child.IconKey = child.HasChildNodes ? "FolderTreeIcon" : "MusicFolderIcon"; 
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

            CategoryDTO? categoryDto = parentCategory.Tag as CategoryDTO ?? null;
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
                        NavigationCommand = new CategoryNavigationCommand(navigationService, (int)childCategory.Id),
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
            foreach (var tag in tags)
            {
                var childTag = new MenuItemModel
                {
                    DisplayName = tag.Name,
                    HasChildNodes = false,
                    IconKey = "TagsIcon",
                    NavigationCommand = new RelayCommand(() =>
                    {
                        if (tag.Id.HasValue)
                        {
                            navigationService.NavigateTo<TagsViewModel>(tag.Id);
                        }
                    }),
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
