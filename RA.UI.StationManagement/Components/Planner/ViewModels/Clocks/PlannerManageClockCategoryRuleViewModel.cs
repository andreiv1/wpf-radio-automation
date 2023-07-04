using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.DAL;
using RA.DTO;
using RA.Logic;
using RA.UI.Core.Services;
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
        private readonly IDispatcherService dispatcherService;
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
        public ObservableCollection<TagValueDTO> SelectedGenres { get; set; } = new();
        public ObservableCollection<TagValueDTO> Languages { get; set; } = new();
        public ObservableCollection<TagValueDTO> SelectedLanguages { get; set; } = new();
        public ObservableCollection<TagValueDTO> Moods { get; set; } = new();
        public ObservableCollection<TagValueDTO> SelectedMoods { get; set; } = new();
        public PlannerManageClockCategoryRuleViewModel(IWindowService windowService,
            IDispatcherService dispatcherService,
                                                       ICategoriesService categoriesService,
                                                       IClocksService clocksService,
                                                       ITagsService tagsService,
                                                       int clockId) : base(windowService)
        {
            this.dispatcherService = dispatcherService;
            this.categoriesService = categoriesService;
            this.clocksService = clocksService;
            this.tagsService = tagsService;
            this.clockId = clockId;
            ManageModel = new ManageClockCategoryModel();
            _ = FetchTags();
            InitTagsCollectionEvents();
        }

        //TODO: Edit ctr
        public PlannerManageClockCategoryRuleViewModel(IWindowService windowService,
            IDispatcherService dispatcherService,
                                                       ICategoriesService categoriesService,
                                                       IClocksService clocksService,
                                                       ITagsService tagsService,
                                                       int clockId, int clockItemId) : base(windowService)
        {
            this.dispatcherService = dispatcherService;
            this.categoriesService = categoriesService;
            this.clocksService = clocksService;
            this.tagsService = tagsService;
            this.clockId = clockId;

            var loadTask = LoadModel(clockItemId);
            var fetchTask = FetchTags();

            Task.WhenAll(loadTask, fetchTask)
                .ContinueWith(t => AsignTagsToClockItem());
            InitTagsCollectionEvents();
        }

        private void AsignTagsToClockItem()
        {
            if(ManageModel == null || ManageModel.Tags == null) return;
            foreach (var genre in Genres)
            {
                if (ManageModel.Tags.Where(t => t.TagValueId == genre.Id).Any())
                {
                    dispatcherService.InvokeOnUIThread(() => SelectedGenres.Add(genre));
                }
            }
            foreach (var language in Languages)
            {
                if (ManageModel.Tags.Where(t => t.TagValueId == language.Id).Any())
                {
                    dispatcherService.InvokeOnUIThread(() => SelectedLanguages.Add(language));
                }
            }
            foreach (var mood in Moods)
            {
                if (ManageModel.Tags.Where(t => t.TagValueId == mood.Id).Any())
                {
                    dispatcherService.InvokeOnUIThread(() => SelectedMoods.Add(mood));
                }
            }
        }
        private void InitTagsCollectionEvents()
        {
            SelectedGenres.CollectionChanged += SelectedGenres_CollectionChanged;
            SelectedLanguages.CollectionChanged += SelectedLanguages_CollectionChanged;
            SelectedMoods.CollectionChanged += SelectedMoods_CollectionChanged;
        }

        private void SelectedMoods_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ProcessTags_CollectionChnaged(e);
        }

        private void SelectedLanguages_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ProcessTags_CollectionChnaged(e);
        }

        private void SelectedGenres_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ProcessTags_CollectionChnaged(e);
        }

        private void ProcessTags_CollectionChnaged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                //var removedItems = e.OldItems;
                //if (removedItems == null || Track == null || Track.Tags == null)
                //    return;

                //foreach (var item in removedItems)
                //{
                //    TagValueDTO? tagValue = item as TagValueDTO;
                //    if (tagValue != null)
                //    {
                //        var toDelete = Track.Tags.Where(t => t.TagValueId == tagValue.Id).First();
                //        Track.Tags.Remove(toDelete);
                //    }
                //}
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                //var addedItems = e.NewItems;
                //if (addedItems == null || Track == null || Track.Tags == null)
                //    return;

                //foreach (var item in addedItems)
                //{
                //    TagValueDTO? tagValue = item as TagValueDTO;
                //    if (tagValue != null)
                //    {
                //        var alreadyExists = Track.Tags.Any(t => t.TagValueId == tagValue.Id);
                //        if (!alreadyExists)
                //        {
                //            Track.Tags.Add(new TrackTagDTO() { TagCategoryId = tagValue.TagCategoryId, TagValueId = tagValue.Id, TrackId = Track.Id });
                //        }

                //    }
                //}
            }
        }

        public async Task AddClockItem(int orderIndex)
        {
            if (SelectedCategory == null) return;
            if (Result == RADialogResult.Cancel) return;
            var newClockItem = new ClockItemCategoryDTO()
            {
                OrderIndex = orderIndex,
                CategoryId = SelectedCategory.Id,
                ClockId = clockId,
                MinReleaseDate = ManageModel.FromReleaseDate,
                MaxReleaseDate = ManageModel.ToReleaseDate,
                IsFiller = ManageModel.IsFiller,
            };

            if (ManageModel.EnforceDurationLimits)
            {
                newClockItem.MinDuration = ManageModel.MinDuration;
                newClockItem.MaxDuration = ManageModel.MaxDuration;
            }

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

        protected override void CancelDialog()
        {
            SelectedCategory = null;
            base.CancelDialog();
        }

        //Data fetch
        
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
        
    }
}
