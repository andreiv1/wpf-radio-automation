using CommunityToolkit.Mvvm.ComponentModel;
using RA.Database.Models.Enums;
using RA.DTO;
using RA.DTO.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace RA.UI.StationManagement.Components.Planner.ViewModels.MainContent.Models
{
    public partial class ClockItemModel : ObservableObject
    {
        public ClockItemBaseDTO Item { get; private set; }

        public String ItemDetails
        {
            get
            {
                if (Item is ClockItemCategoryDTO category)
                {
                    return $"{category.CategoryName}";
                }

                if (Item is ClockItemEventDTO eventItem){
                    return eventItem.EventLabel;
                }

                if(Item is ClockItemTrackDTO trackItem)
                {
                    string result = trackItem.Artists ?? "";
                    if(result == string.Empty)
                    {
                        result = trackItem.Title ?? "-";
                    }
                    else
                    {
                        result += $" - {trackItem.Title}";
                    }
                    return result;
                }

                return $"Unknown";
            }
        }
        public String DisplayName
        {
            get
            {
                return "Test";
            }
        }
        public String ItemType
        {
            get
            {
                if(Item is ClockItemCategoryDTO itemCategory)
                {
                    return "Category";
                }
                if(Item is ClockItemEventDTO itemEvent)
                {
                    switch (itemEvent.EventType)
                    {
                        case EventType.Marker:
                            return "EventMarker";
                        case EventType.FixedBreak:
                            return "EventFixedBreak";
                        case EventType.DynamicBreak:
                            return "EventDynamicBreak";
                        default:
                            return "EventDefault";
                    }
                }
                if(Item is ClockItemTrackDTO itemTrack)
                {
                    return itemTrack.TrackType.ToString();
                }
                return "Unknown";
            }
        }

        public Brush? ItemColor
        {
            get
            {
                if (Item is ClockItemCategoryDTO itemCategory && itemCategory.CategoryColor != null)
                {
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString(itemCategory.CategoryColor));
                }
                return new SolidColorBrush(Color.FromRgb(0, 0, 0));
            }
        }


        [ObservableProperty]
        private TimeSpan startTime;

        [ObservableProperty]
        private TimeSpan duration;
        public ClockItemModel(ClockItemBaseDTO item)
        {
            Item = item;
        }
    }
}
