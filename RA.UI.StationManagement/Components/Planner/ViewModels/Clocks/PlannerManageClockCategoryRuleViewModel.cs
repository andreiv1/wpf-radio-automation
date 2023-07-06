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
using System.Collections.ObjectModel;
using System.Linq;
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
        private readonly int clockItemId;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(FinishDialogCommand))]
        private CategoryHierarchyDTO? selectedCategory;

        [ObservableProperty]
        private ManageClockCategoryModel categoryModel;

        [ObservableProperty]
        private int noOfTracksMatchingConditions = 0;

        public ObservableCollection<TagValueDTO> Genres { get; set; } = new();
        public ObservableCollection<TagValueDTO> SelectedGenres { get; set; } = new();
        public ObservableCollection<TagValueDTO> Languages { get; set; } = new();
        public ObservableCollection<TagValueDTO> SelectedLanguages { get; set; } = new();
        public ObservableCollection<TagValueDTO> Moods { get; set; } = new();
        public ObservableCollection<TagValueDTO> SelectedMoods { get; set; } = new();

        public bool IsEditing { get; private set; } = false;
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
            CategoryModel = new ManageClockCategoryModel();
            _ = FetchTags();
            InitTagsCollectionEvents();
        }

        //TODO: Edit ctr
        public PlannerManageClockCategoryRuleViewModel(IWindowService windowService,
                                                       IDispatcherService dispatcherService,
                                                       ICategoriesService categoriesService,
                                                       IClocksService clocksService,
                                                       ITagsService tagsService,
                                                       int clockId,
                                                       int clockItemId) : base(windowService)
        {
            this.dispatcherService = dispatcherService;
            this.categoriesService = categoriesService;
            this.clocksService = clocksService;
            this.tagsService = tagsService;
            this.clockId = clockId;
            this.clockItemId = clockItemId;
            IsEditing = true;
            var loadTask = LoadModel(clockItemId);
            var fetchTask = FetchTags();

            Task.WhenAll(loadTask, fetchTask)
                .ContinueWith(t => AsignTagsToClockItem());
            InitTagsCollectionEvents();
        }

        private void AsignTagsToClockItem()
        {
            if(CategoryModel == null || CategoryModel.Tags == null) return;
            foreach (var genre in Genres)
            {
                if (CategoryModel.Tags.Where(t => t.TagValueId == genre.Id).Any())
                {
                    dispatcherService.InvokeOnUIThread(() => SelectedGenres.Add(genre));
                }
            }
            foreach (var language in Languages)
            {
                if (CategoryModel.Tags.Where(t => t.TagValueId == language.Id).Any())
                {
                    dispatcherService.InvokeOnUIThread(() => SelectedLanguages.Add(language));
                }
            }
            foreach (var mood in Moods)
            {
                if (CategoryModel.Tags.Where(t => t.TagValueId == mood.Id).Any())
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
            ProcessTags_CollectionChanged(e);
        }

        private void SelectedLanguages_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ProcessTags_CollectionChanged(e);
        }

        private void SelectedGenres_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ProcessTags_CollectionChanged(e);
        }

        private void ProcessTags_CollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                var removedItems = e.OldItems;
                if (removedItems == null || CategoryModel == null || CategoryModel.Tags == null) return;
                foreach(var item in removedItems)
                {
                    TagValueDTO? tagValue = item as TagValueDTO;
                    if (tagValue != null)
                    {
                        var toDelete = CategoryModel.Tags.Where(x => x.TagValueId == tagValue.Id).First();
                        CategoryModel.Tags.Remove(toDelete);
                    }
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                var addedItems = e.NewItems;
                if (addedItems == null || CategoryModel == null || CategoryModel.Tags == null)
                    return;

                foreach (var item in addedItems)
                {
                    TagValueDTO? tagValue = item as TagValueDTO;
                    if (tagValue != null)
                    {
                        var alreadyExists = CategoryModel.Tags.Any(t => t.TagValueId == tagValue.Id);
                        if (!alreadyExists)
                        {
            
                            CategoryModel.Tags.Add(new ClockItemCategoryTagDTO()
                            {
                                ClockItemId = clockItemId,
                                TagValueId = tagValue.Id
                            });
                        }

                    }
                }
            }
        }

        public async Task AddClockItem(int orderIndex)
        {
            if (SelectedCategory == null) return;
            if (Result == RADialogResult.Cancel) return;

            var newClockItemDto = ManageClockCategoryModel.ToDto(CategoryModel);
            newClockItemDto.CategoryId = SelectedCategory.Id;
            newClockItemDto.ClockId = clockId;
            newClockItemDto.OrderIndex = orderIndex;
            await clocksService.AddClockItem(newClockItemDto);
        }

        public async Task AddClockItemToEvent(int eventOrderIndex, int eventId)
        {
            if(SelectedCategory == null) return;
            if (Result == RADialogResult.Cancel) return;

            var newClockItemDto = ManageClockCategoryModel.ToDto(CategoryModel);
            newClockItemDto.CategoryId = SelectedCategory.Id;
            newClockItemDto.ClockId = clockId;
            newClockItemDto.OrderIndex = -1;
            newClockItemDto.EventOrderIndex = eventOrderIndex;
            newClockItemDto.ClockItemEventId = eventId;
            await clocksService.AddClockItem(newClockItemDto);

        }

        public async Task UpdateClockItem()
        {
            if (SelectedCategory == null) return;
            var dto = ManageClockCategoryModel.ToDto(CategoryModel);
            dto.CategoryId = SelectedCategory.Id;
            dto.Id = clockItemId;
            dto.ClockId = clockId;

            //dto.Or
           
            await clocksService.UpdateClockItem(dto);
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
            CategoryModel = ManageClockCategoryModel.FromDto(dto);
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
