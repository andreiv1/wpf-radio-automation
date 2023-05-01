using CommunityToolkit.Mvvm.ComponentModel;
using RA.DAL;
using RA.Database.Models;
using RA.DTO;
using RA.UI.Core.Services;
using RA.UI.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.MediaLibrary.ViewModels.MainContent
{
    public partial class TagsViewModel : ViewModelBase
    {
        private readonly IDispatcherService dispatcherService;
        private readonly ITagsService tagsService;

        public ObservableCollection<TagCategoryDto> TagCategories { get; private set; } = new();
        public ObservableCollection<TagValueDto> TagValues { get; private set; } = new();

        [ObservableProperty]
        private TagCategoryDto? selectedTagCategory;
        partial void OnSelectedTagCategoryChanged(TagCategoryDto? value)
        {
            if(value != null && value.Id.HasValue)
            {
                var loadTagsTask = LoadTagValues((int)value.Id);
                loadTagsTask.ContinueWith((task) =>
                {
                    IsAnyTagValues = TagValues.Any();
                });
            }
        }

        [ObservableProperty]
        private bool isAnyTagValues = false;

        [ObservableProperty]
        private bool isTagCategoryValuesLoading = false;

        public TagsViewModel(IDispatcherService dispatcherService, ITagsService tagsService)
        {
            this.dispatcherService = dispatcherService;
            this.tagsService = tagsService;
            _ = LoadTagCategories();
        }

        public TagsViewModel(IDispatcherService dispatcherService, ITagsService tagsService, int tagCategoryId)
        {
            this.dispatcherService = dispatcherService;
            this.tagsService = tagsService;
            var loadTagCategoriesTask = LoadTagCategories();
            loadTagCategoriesTask.ContinueWith((task) =>
            {
                SelectedTagCategory = TagCategories.Where(t => t.Id == tagCategoryId).FirstOrDefault();
            });
        }

        private async Task LoadTagCategories()
        {
            IsMainDataLoading = true;
            var tags = await tagsService.GetTagCategoriesAsync();
            TagCategories.Clear();
            foreach (var tag in tags)
            {
                dispatcherService.InvokeOnUIThread(() =>
                {
                    TagCategories.Add(tag);
                });
            }
            IsMainDataLoading = false;
        }

        private async Task LoadTagValues(int tagCategoryId)
        {
            IsTagCategoryValuesLoading = true;
            var tagValues = await tagsService.GetTagValuesByCategoryAsync(tagCategoryId);
            TagValues.Clear();
            foreach(var value in tagValues)
            {
                TagValues.Add(value);
            }
            IsTagCategoryValuesLoading = false;
        }
    }
}
