using CommunityToolkit.Mvvm.Input;
using RA.DAL;
using RA.UI.Core.Services;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Dialogs.CategorySelectDialog;
using Syncfusion.UI.Xaml.TreeView.Engine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Dialogs.TrackSelectDialog
{
    public partial class TrackSelectViewModel : DialogViewModelBase
    {
        private readonly IDispatcherService dispatcherService;
        private readonly ICategoriesService categoriesService;

        private CategorySelectModel? selectedCategory;
        public ObservableCollection<CategorySelectModel> CategoryItems { get; set; } = new();
        public TrackSelectViewModel(IWindowService windowService, IDispatcherService dispatcherService, 
            ICategoriesService categoriesService) : base(windowService)
        {
            this.dispatcherService = dispatcherService;
            this.categoriesService = categoriesService;

            Task.Run(() => LoadRootCategories());
        }

        protected override bool CanFinishDialog()
        {
            return false;
        }

        [RelayCommand(CanExecute = nameof(CanExecuteOnDemandLoading))]
        private void ExecuteOnDemandLoading(object obj)
        {
            var node = obj as TreeViewNode;
            node!.ShowExpanderAnimation = true;

            dispatcherService.InvokeOnUIThread(new Action(
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
                    dispatcherService.InvokeOnUIThread(() =>
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

            dispatcherService.InvokeOnUIThread(() =>
            {
                parentNode.PopulateChildNodes(childItems);
            });

        }
    }
}
