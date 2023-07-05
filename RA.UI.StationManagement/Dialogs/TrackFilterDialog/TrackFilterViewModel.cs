using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using Syncfusion.Data;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;

namespace RA.UI.StationManagement.Dialogs.TrackFilterDialog
{
    public enum FilterLabelType
    {
        Album,
        Category,
        DateAdded,
        DateModified,
        Duration,
        ReleaseDate,
        Status,
        Type,
        Title
    }

    public enum FilterControlType
    {
        Textbox,
        TimeSpan,
        DatePicker,
        CategoryPicker,
        StatusPicker,
        TypePicker,
    }

    public enum FilterOperator
    {
        Equals,
        LessThan,
        GreaterThan,
        Like
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
                    KeyValuePair.Create(FilterLabelType.Category, FilterControlType.Textbox),
                    KeyValuePair.Create(FilterLabelType.DateAdded, FilterControlType.DatePicker),
                    KeyValuePair.Create(FilterLabelType.DateModified, FilterControlType.DatePicker),
                    KeyValuePair.Create(FilterLabelType.Duration, FilterControlType.Textbox),
                    KeyValuePair.Create(FilterLabelType.ReleaseDate, FilterControlType.Textbox),
                    KeyValuePair.Create(FilterLabelType.Status, FilterControlType.Textbox),
                    KeyValuePair.Create(FilterLabelType.Type, FilterControlType.Textbox),
                    KeyValuePair.Create(FilterLabelType.Title, FilterControlType.Textbox),
            }
        );

        [ObservableProperty]
        private FilterLabelType selectedLabelType;

        partial void OnSelectedLabelTypeChanged(FilterLabelType value)
        {
            if(allowedOperatorsByLabel.TryGetValue(value, out var existingOperators))
            {
                Operators.Clear();
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
        private String? textValue;

        partial void OnTextValueChanged(string? value)
        {
            DateValue = null;
        }

        [ObservableProperty]
        private DateTime? dateValue;

        partial void OnDateValueChanged(DateTime? value)
        {
            TextValue = null;
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

        protected override bool CanFinishDialog()
        {
            return false;
        }
    }
}
