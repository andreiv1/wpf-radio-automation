using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.DAL;
using RA.DTO;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Dialogs.CategorySelectDialog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.Planner.ViewModels.Clocks
{
    public partial class PlannerManageClockCategoryRuleViewModel : DialogViewModelBase
    {
        private readonly ICategoriesService categoriesService;
        private readonly ITagsService tagsService;
        private readonly int clockId;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(FinishDialogCommand))]
        private CategoryHierarchyDTO? selectedCategory;

        [ObservableProperty]
        private TimeSpan artistSeparation;

        [ObservableProperty]
        private TimeSpan titleSeparation;

        [ObservableProperty]
        private TimeSpan trackSeparation;

        public ObservableCollection<TagValueDTO> Genres { get; set; } = new();
        public ObservableCollection<TagValueDTO> Languages { get; set; } = new();
        public ObservableCollection<TagValueDTO> Moods { get; set; } = new();
        public PlannerManageClockCategoryRuleViewModel(IWindowService windowService,
                                                       ICategoriesService categoriesService,
                                                       ITagsService tagsService,
                                                       int clockId) : base(windowService)
        {
            this.categoriesService = categoriesService;
            this.tagsService = tagsService;
            this.clockId = clockId;
            _ = FetchTags();
        }

        protected override bool CanFinishDialog()
        {
            return SelectedCategory != null;
        }

        #region Data fetch
        private async Task FetchTags()
        {
            foreach(var genre in await tagsService.GetTagValuesByCategoryNameAsync("Genre"))
            {
                Genres.Add(genre);
            }

            foreach(var language in await tagsService.GetTagValuesByCategoryNameAsync("Language"))
            {
                Languages.Add(language);
            }

            foreach(var mood in await tagsService.GetTagValuesByCategoryNameAsync("Mood"))
            {
                Moods.Add(mood);
            }
        }
        #endregion
        #region Commands
        [RelayCommand]
        private async void OpenPickCategory()
        {
            var vm = windowService.ShowDialog<CategorySelectViewModel>();
            if(vm.SelectedCategory != null)
            {
                SelectedCategory = await categoriesService.GetCategoryHierarchy(vm.SelectedCategory.CategoryId);
            }
        }
        #endregion
    }
}
