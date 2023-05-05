using CommunityToolkit.Mvvm.ComponentModel;
using RA.DAL;
using RA.DTO;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.MediaLibrary.ViewModels.Categories
{
    public partial class MediaLibraryManageCategoryViewModel : DialogViewModelBase
    {
        [ObservableProperty]
        private CategoryDTO? category;
        private readonly IWindowService windowService;
        private readonly ICategoriesService categoriesService;

        public string? ParentName { get; }

        public MediaLibraryManageCategoryViewModel(IWindowService windowService,
                                                   ICategoriesService categoriesService) : base(windowService)
        {
            this.windowService = windowService;
            this.categoriesService = categoriesService;
            Category = new();
        }

        public MediaLibraryManageCategoryViewModel(IWindowService windowService,
                                                   ICategoriesService categoriesService,
                                                   int categoryId) : base(windowService)
        {
            this.windowService = windowService;
            this.categoriesService = categoriesService;
            _ = LoadCategory(categoryId);
        }

        public MediaLibraryManageCategoryViewModel(IWindowService windowService,
                                                   ICategoriesService categoriesService,
                                                   int parentCategoryId, string parentName) : base(windowService)
        {
            this.windowService = windowService;
            this.categoriesService = categoriesService;
            ParentName = parentName;
            Category = new()
            {
                ParentId = parentCategoryId,
            };
        }

        private async Task LoadCategory(int categoryId)
        {
            Category = await categoriesService.GetCategory(categoryId);
        }

        protected override void FinishDialog()
        {
            if(Category != null)
            {
                if (Category.Id.HasValue)
                {
                    //to do update
                } else
                {
                    //to do insert
                    _ = categoriesService.AddCategory(Category);
                }
            }
            base.FinishDialog();
        }
        protected override bool CanFinishDialog()
        {
            return true;
        }
    }
}
