using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.DAL;
using RA.DTO;
using RA.Logic;
using RA.UI.Core.Services;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Dialogs.CategorySelectDialog;
using Syncfusion.UI.Xaml.TreeView.Engine;
using Syncfusion.Windows.Controls.Input;
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
        private readonly ITracksService tracksService;
        private CategorySelectModel? selectedCategory;
        public ObservableCollection<CategorySelectModel> CategoryItems { get; set; } = new();

        #region Constructor
        public TrackSelectViewModel(IWindowService windowService,
                                    IDispatcherService dispatcherService,
                                    ICategoriesService categoriesService,
                                    ITracksService tracksService) : base(windowService)
        {
            this.dispatcherService = dispatcherService;
            this.categoriesService = categoriesService;
            this.tracksService = tracksService;
            Task.Run(() => LoadRootCategories());
            Task.Run(() => LoadTracks(0, 100));
        }
        #endregion

        public ObservableCollection<TrackListingDTO> Tracks { get; set; } = new();

        [ObservableProperty]
        private int totalTracks = 0;

        [ObservableProperty]
        private int pages;

        private const int tracksPerPage = 100;

        #region Data fetching
        public async Task LoadTracks(int skip, int take)
        {
            IsMainDataLoading = true;
            Tracks.Clear();
            TotalTracks = await tracksService.GetTrackCountAsync();
            Pages = TotalTracks > 0 ? (TotalTracks - 1) / tracksPerPage + 1 : 0;
            var tracks = await tracksService.GetTrackListAsync(skip, take);

            foreach (var track in tracks.ToList())
            {
                Tracks.Add(track);
            }
            IsMainDataLoading = false;
        }
        #endregion
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
