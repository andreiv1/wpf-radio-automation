using CommunityToolkit.Mvvm.ComponentModel;
using RA.Database.Models.Enums;
using RA.DTO;
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
        public int? TrackId { get; set; }
        public int? CategoryId { get; set; }
        public String CategoryName { get; set; } = String.Empty;
        public int? ClockId { get; set; }
        public int OrderIndex { get; set; }
        public EventType? EventType { get; set; }
        public string? EventLabel { get; set; }
        public TimeSpan? EstimatedEventDuration { get; set; }
        public String ItemDetails
        {
            get
            {
                if (CategoryId.HasValue)
                {
                    return $"From category: {CategoryName}";
                }

                if (TrackId.HasValue)
                {
                    return $"Track: Id={TrackId}";
                }

                if (EventType.HasValue)
                {
                    return $"[{EventType.ToString()}] {EventLabel}";
                }

                return $"Unknown item";
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

        public static ClockItemModel FromDto(ClockItemDTO dto)
        {
            var model = new ClockItemModel()
            {
                Id = dto.Id,
                TrackId = dto.TrackId,
                CategoryId = dto.CategoryId,
                CategoryName = dto.CategoryName,
                EventType = dto.EventType,
                EventLabel = dto.EventLabel,
                EstimatedEventDuration = dto.EstimatedEventDuration,
                ClockId = dto.ClockId,
                OrderIndex = dto.OrderIndex,
            };

            return model;
        }

        public static ClockItemDTO ToDto(ClockItemModel model)
        {
            return new ClockItemDTO()
            {
                Id = model.Id,
                TrackId = model.TrackId,
                CategoryId = model.CategoryId,
                ClockId = model.ClockId,
                OrderIndex = model.OrderIndex,
            };
        }
    }
}
