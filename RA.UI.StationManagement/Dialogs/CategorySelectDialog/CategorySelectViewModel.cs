using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.DAL;
using RA.Database.Models;
using RA.DTO;
using RA.UI.Core.Services;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Services;
using Syncfusion.UI.Xaml.TreeView.Engine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace RA.UI.StationManagement.Dialogs.CategorySelectDialog
{
    public partial class CategorySelectModel : ObservableObject
    {
        [ObservableProperty]
        private string name = "";

        [ObservableProperty]
        private int categoryId;

        [ObservableProperty]
        private bool hasChild = false;

        public ObservableCollection<CategorySelectModel>? Children { get; set; }

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
    public partial class CategorySelectViewModel : DialogViewModelBase
    {
        private readonly IDispatcherService dispatcher;
        private readonly ICategoriesService categoriesService;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(FinishDialogCommand))]
        private CategorySelectModel? selectedCategory;
        public ObservableCollection<CategorySelectModel> CategoryItems { get; set; } = new();
        public CategorySelectViewModel(IWindowService windowService, IDispatcherService dispatcher,
            ICategoriesService categoriesService) : base(windowService)
        {
            DialogName = "Select category";
            this.dispatcher = dispatcher;
            this.categoriesService = categoriesService;

            Task.Run(() => LoadRootCategories());
        }

        [RelayCommand(CanExecute = nameof(CanExecuteOnDemandLoading))]
        private void ExecuteOnDemandLoading(object obj)
        {
            var node = obj as TreeViewNode;
            node!.ShowExpanderAnimation = true;

            dispatcher.InvokeOnUIThread(new Action(
                async () =>
                {
                    await Task.Delay(300);
                    await Task.Run(() => LoadChildCategories(node));
                    node!.ShowExpanderAnimation = false;
                    node!.IsExpanded = true;
                }));

        }
        private bool CanExecuteOnDemandLoading(object sender)
        {
            var hasChildNodes = ((sender as TreeViewNode)!.Content as CategorySelectModel)!.HasChild;
            return hasChildNodes;
        }

        private async Task LoadRootCategories()
        {
            var categories = await categoriesService.GetRootCategoriesAsync();
            foreach (var category in categories)
            {
                if (category.Id.HasValue)
                {
                    var child = new CategorySelectModel
                    {
                        Name = category.Name,
                        HasChild = await categoriesService.HasCategoryChildren(category.Id.Value),
                        IconKey = "FolderTreeIcon",
                        CategoryId = category.Id.Value,
                    };
                    dispatcher.InvokeOnUIThread(() =>
                    {
                        CategoryItems.Add(child);
                    });
                }
            }
        }

        private async Task LoadChildCategories(TreeViewNode parentNode)
        {
            var parentCategory = parentNode.Content as CategorySelectModel;
            if (parentCategory == null)
            {
                return;
            }


            var childCategories = await categoriesService.GetChildrenCategoriesAsync(parentCategory.CategoryId);
            if (childCategories == null)
            {
                return;
            }

            var childItems = new ObservableCollection<CategorySelectModel>();
            foreach (var childCategory in childCategories)
            {
                var childItem = new CategorySelectModel
                {
                    Name = childCategory.Name,
                    HasChild = await categoriesService.HasCategoryChildren(childCategory.Id!.Value),
                    IconKey = "FolderTreeIcon",
                    CategoryId = childCategory.Id.Value,
                };

                childItem.IconKey = childItem.HasChild ? "FolderTreeIcon" : "MusicFolderIcon";

                childItems.Add(childItem);
            }

            dispatcher.InvokeOnUIThread(() =>
            {
                parentNode.PopulateChildNodes(childItems);
            });

        }

        protected override void CancelDialog()
        {
            base.CancelDialog();
            SelectedCategory = null;
        }

        protected override bool CanFinishDialog()
        {
            return SelectedCategory is not null;
        }
    }
}
