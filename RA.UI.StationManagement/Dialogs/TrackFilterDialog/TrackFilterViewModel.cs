using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.DAL.Models;
using RA.Database.Models.Enums;
using RA.DTO;
using RA.Logic;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;

namespace RA.UI.StationManagement.Dialogs.TrackFilterDialog
{
    public enum FilterControlType
    {
        Textbox,
        TimeSpan,
        DatePicker,
        CategoryPicker,
        StatusPicker,
        TypePicker,
    }

    public partial class FilterModel : ObservableObject
    {
        public static ImmutableArray<FilterOperator> operatorsEqLike = ImmutableArray.Create(FilterOperator.Equals, FilterOperator.Like);
        public static ImmutableArray<FilterOperator> operatorsEqLtGt = ImmutableArray.Create(FilterOperator.Equals, FilterOperator.LessThan, FilterOperator.GreaterThan);
        public static ImmutableArray<FilterOperator> operatorsEq = ImmutableArray.Create(FilterOperator.Equals);

        public static readonly ImmutableDictionary<FilterLabelType, ImmutableArray<FilterOperator>> allowedOperatorsByLabel = ImmutableDictionary.CreateRange(
           new[]
           {
                    KeyValuePair.Create(FilterLabelType.Album, FilterModel.operatorsEqLike),
                    KeyValuePair.Create(FilterLabelType.Category, FilterModel.operatorsEq),
                    KeyValuePair.Create(FilterLabelType.DateAdded, FilterModel.operatorsEqLtGt),
                    KeyValuePair.Create(FilterLabelType.DateModified, FilterModel.operatorsEqLtGt),
                    KeyValuePair.Create(FilterLabelType.Duration, FilterModel.operatorsEqLtGt),
                    KeyValuePair.Create(FilterLabelType.ReleaseDate, FilterModel.operatorsEqLtGt),
                    KeyValuePair.Create(FilterLabelType.Status, FilterModel.operatorsEq),
                    KeyValuePair.Create(FilterLabelType.Type, FilterModel.operatorsEq),
                    KeyValuePair.Create(FilterLabelType.Title, FilterModel.operatorsEqLike),
           });

        public static readonly ImmutableDictionary<FilterLabelType, FilterControlType> controlByLabel = ImmutableDictionary.CreateRange(
            new[]
            {
                KeyValuePair.Create(FilterLabelType.Album, FilterControlType.Textbox),
                    KeyValuePair.Create(FilterLabelType.Category, FilterControlType.CategoryPicker),
                    KeyValuePair.Create(FilterLabelType.DateAdded, FilterControlType.DatePicker),
                    KeyValuePair.Create(FilterLabelType.DateModified, FilterControlType.DatePicker),
                    KeyValuePair.Create(FilterLabelType.Duration, FilterControlType.TimeSpan),
                    KeyValuePair.Create(FilterLabelType.ReleaseDate, FilterControlType.DatePicker),
                    KeyValuePair.Create(FilterLabelType.Status, FilterControlType.StatusPicker),
                    KeyValuePair.Create(FilterLabelType.Type, FilterControlType.TypePicker),
                    KeyValuePair.Create(FilterLabelType.Title, FilterControlType.Textbox),
            }
        );

        public static TrackStatus[] TrackStatuses => Enum.GetValues(typeof(TrackStatus)).Cast<TrackStatus>().ToArray();

        public static TrackType[] TrackTypes => Enum.GetValues(typeof(TrackType)).Cast<TrackType>().ToArray();

        [ObservableProperty]
        private FilterLabelType selectedLabelType;

        partial void OnSelectedLabelTypeChanged(FilterLabelType value)
        {
            Operators.Clear();
            if (allowedOperatorsByLabel.TryGetValue(value, out var existingOperators))
            {
                foreach(var op in existingOperators)
                {
                    Operators.Add(op);
                }
                SelectedOperator = Operators.First();
            };

            if(controlByLabel.TryGetValue(value, out var controlItem))
            {
                ControlType = controlItem;
            }
            ClearAllValues();
        }

        public ObservableCollection<FilterOperator> Operators { get; private set; } = new ObservableCollection<FilterOperator>();

        [ObservableProperty]
        private FilterOperator selectedOperator;

        [ObservableProperty]
        private FilterControlType controlType;

        public FilterModel()
        {
            SelectedLabelType = FilterLabelType.Album;
            OnSelectedLabelTypeChanged(SelectedLabelType);
        }

        [ObservableProperty]
        private string? textValue;

        [ObservableProperty]
        private TimeSpan? timeSpanValue;

        [ObservableProperty]
        private DateTime? dateValue;

        [ObservableProperty]
        private CategoryDTO? categoryValue;

        [ObservableProperty]
        private TrackStatus? trackStatusValue;

        [ObservableProperty]
        private TrackType? trackTypeValue;
        private void ClearAllValues()
        {
            TextValue = null;
            TimeSpanValue = null;
            DateValue = null;
            CategoryValue = null;
            TrackStatusValue = null;
            TrackTypeValue = null;
        }

        [RelayCommand]
        private void OpenPickCategory()
        {
            throw new NotImplementedException();
        }


    }
        
    public partial class TrackFilterViewModel : DialogViewModelBase
    {

        public static FilterLabelType[] FilterLabelTypes { get; } = (FilterLabelType[])Enum.GetValues(typeof(FilterLabelType)).Cast<FilterLabelType>()
            .OrderBy(type => type.ToString())
            .ToArray();
        public ObservableCollection<FilterModel> Filters { get; set; } = new()
        {
            new FilterModel()
        };

        public List<TrackFilterCondition>? Conditions { get; private set; } = null;

        [ObservableProperty]
        private bool isMatchAll = true;

        [ObservableProperty]
        private bool isMatchAny = false;

        public TrackFilterViewModel(IWindowService windowService) : base(windowService) 
        {
        }

        [RelayCommand]
        private void AddFilter()
        {
            Filters.Add(new FilterModel());
        }

        [RelayCommand]
        private void RemoveFilter(object parameter)
        {
            if (parameter is FilterModel filter)
            {
                Filters.Remove(filter);
            }
        }

        private void PreviewFilters()
        {
            foreach(var filter in Filters)
            {
                DebugHelper.WriteLine(this, $"{filter.SelectedLabelType} - {filter.SelectedOperator}");
                if(filter.TextValue != null)
                {
                    DebugHelper.WriteLine(this, $"{filter.TextValue}");
                } else if(filter.TimeSpanValue != null)
                {
                    DebugHelper.WriteLine(this, $"{filter.TimeSpanValue}");
                } else if(filter.DateValue != null)
                {
                    DebugHelper.WriteLine(this, $"{filter.DateValue}");
                } else if(filter.CategoryValue != null)
                {
                    DebugHelper.WriteLine(this, $"{filter.CategoryValue.Id} - {filter.CategoryValue.Name}");
                } else if(filter.TrackStatusValue != null)
                {
                    DebugHelper.WriteLine(this, $"{filter.TrackStatusValue.ToString()}");
                } else if(filter.TrackTypeValue != null)
                {
                    DebugHelper.WriteLine(this, $"{filter.TrackTypeValue.ToString()}");
                }
            }
        }

        private void CreateConditions()
        {
            Conditions = new();
            foreach(var filter in Filters)
            {
                object? value = null;
                if (filter.TextValue != null)
                {
                    value = filter.TextValue;
                }
                else if (filter.TimeSpanValue != null)
                {
                    value = filter.TimeSpanValue;
                }
                else if (filter.DateValue != null)
                {
                    value = filter.DateValue;
                }
                else if (filter.CategoryValue != null)
                {
                    value = filter.CategoryValue;
                }
                else if (filter.TrackStatusValue != null)
                {
                    value = filter.TrackStatusValue;
                }
                else if (filter.TrackTypeValue != null)
                {
                    value = filter.TrackTypeValue;
                }
                Conditions.Add(new TrackFilterCondition(filter.SelectedLabelType, filter.SelectedOperator, value));
            }
        }

        protected override void FinishDialog()
        {
            PreviewFilters();
            CreateConditions();
            base.FinishDialog();
        }
        protected override bool CanFinishDialog()
        {
            return true;
        }
    }
}
