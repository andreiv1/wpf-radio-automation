using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.DAL;
using RA.DTO;
using RA.Logic;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.Planner.ViewModels.Clocks.Models;
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
        private readonly IClocksService clocksService;
        private readonly ITagsService tagsService;
        private readonly int clockId;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(FinishDialogCommand))]
        private CategoryHierarchyDTO? selectedCategory;

        [ObservableProperty]
        private ManageClockCategoryModel manageModel;

        [ObservableProperty]
        private int noOfTracksMatchingConditions = 0;

        public ObservableCollection<TagValueDTO> Genres { get; set; } = new();
        public ObservableCollection<TagValueDTO> Languages { get; set; } = new();
        public ObservableCollection<TagValueDTO> Moods { get; set; } = new();
        public PlannerManageClockCategoryRuleViewModel(IWindowService windowService,
                                                       ICategoriesService categoriesService,
                                                       IClocksService clocksService,
                                                       ITagsService tagsService,
                                                       int clockId) : base(windowService)
        {
            this.categoriesService = categoriesService;
            this.clocksService = clocksService;
            this.tagsService = tagsService;
            this.clockId = clockId;
            ManageModel = new ManageClockCategoryModel();
            _ = FetchTags();
        }

        public PlannerManageClockCategoryRuleViewModel(IWindowService windowService,
                                                       ICategoriesService categoriesService,
                                                       IClocksService clocksService,
                                                       ITagsService tagsService,
                                                       int clockId, int clockItemId) : base(windowService)
        {
            this.categoriesService = categoriesService;
            this.clocksService = clocksService;
            this.tagsService = tagsService;
            this.clockId = clockId;

            _ = LoadModel(clockItemId);
            _ = FetchTags();
        }

        public async Task AddClockItem(int orderIndex)
        {
            if (SelectedCategory == null) return;

            var newClockItem = new ClockItemCategoryDTO()
            {
                OrderIndex = orderIndex,
                CategoryId = SelectedCategory.Id,
                ClockId = clockId,
                MinBpm = ManageModel.MinBpm,
                MaxBpm = ManageModel.MaxBpm,
                MinDuration = ManageModel.MinDuration,
                MaxDuration = ManageModel.MaxDuration,
                MinReleaseDate = ManageModel.FromReleaseDate,
                MaxReleaseDate = ManageModel.ToReleaseDate,
                IsFiller = ManageModel.IsFiller,
            };

            if(ManageModel.ArtistSeparation?.TotalMinutes > 0)
            {
                newClockItem.ArtistSeparation = (int)ManageModel.ArtistSeparation.Value.TotalMinutes;
            }

            if(ManageModel.TitleSeparation?.TotalMinutes > 0)
            {
                newClockItem.TitleSeparation = (int)ManageModel.TitleSeparation.Value.TotalMinutes;
            }

            if(ManageModel.TrackSeparation?.TotalMinutes > 0)
            {
                newClockItem.TrackSeparation = (int)ManageModel.TrackSeparation.Value.TotalMinutes;
            }

            

            await clocksService.AddClockItem(newClockItem);
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

        private async Task LoadModel(int clockItemId)
        {
            ClockItemCategoryDTO? dto = await clocksService.GetClockItemAsync(clockItemId) as ClockItemCategoryDTO;
            if (dto == null) throw new NullReferenceException($"Clock item must not be null.");
            DebugHelper.WriteLine(this, $"Loaded category clock item {clockItemId}");
            SelectedCategory = await categoriesService.GetCategoryHierarchy(dto.CategoryId.GetValueOrDefault());
            ManageModel = ManageClockCategoryModel.FromDto(dto);
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
                NoOfTracksMatchingConditions = await categoriesService.NoOfTracksMatchingConditions(vm.SelectedCategory.CategoryId);
            }
        }
        #endregion
    }
}
