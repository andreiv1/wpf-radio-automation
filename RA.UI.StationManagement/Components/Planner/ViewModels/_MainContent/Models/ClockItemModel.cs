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
        public int Id { get; private set; }
        
        public ClockItemBaseDTO Item { get; private set; }
        public String ItemDetails
        {
            get
            {
                if (Item is ClockItemCategoryDTO category)
                {
                    return $"From category: {category.CategoryName}";
                }
                //if (CategoryId.HasValue)
                //{
                //    return $"From category: {CategoryName}";
                //}

                //if (TrackId.HasValue)
                //{
                //    return $"Track: Id={TrackId}";
                //}

                //if (EventType.HasValue)
                //{
                //    return $"[{EventType.ToString()}] {EventLabel}";
                //}

                return $"Unknown item";
            }
        }

        public String Display
        {
            get
            {
                return "Test";
            }
        }


        [ObservableProperty]
        private TimeSpan startTime;

        [ObservableProperty]
        private TimeSpan duration;

        [ObservableProperty]
        private SolidColorBrush foregroundColor = new SolidColorBrush(Color.FromRgb(51, 51, 51));

        [ObservableProperty]
        private SolidColorBrush backgroundColor = new SolidColorBrush(Color.FromRgb(51, 51, 51));

        public ClockItemModel(ClockItemBaseDTO item)
        {
            Item = item;
        }
    }
}
