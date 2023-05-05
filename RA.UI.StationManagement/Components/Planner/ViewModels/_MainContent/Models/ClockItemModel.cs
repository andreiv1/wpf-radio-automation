using CommunityToolkit.Mvvm.ComponentModel;
using RA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public String ItemDetails { get; set; } = String.Empty;

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
