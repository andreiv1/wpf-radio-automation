using CommunityToolkit.Mvvm.ComponentModel;
using RA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.Planner.ViewModels.Clocks.Models
{
    public partial class ManageClockCategoryModel : ObservableObject
    {
        [ObservableProperty]
        private DateTime? fromReleaseDate;

        [ObservableProperty] 
        private DateTime? toReleaseDate;

        [ObservableProperty]
        private bool excludeWithoutReleaseDate = false;

        [ObservableProperty]
        private TimeSpan? minDuration = new TimeSpan(0,0,0);

        [ObservableProperty]
        private TimeSpan? maxDuration = new TimeSpan(0,999,0);

        [ObservableProperty]
        private bool enforceDurationLimits = false;

        [ObservableProperty]
        private TimeSpan? artistSeparation = new TimeSpan(0);

        [ObservableProperty]
        private TimeSpan? titleSeparation = new TimeSpan(0);

        [ObservableProperty]
        private TimeSpan? trackSeparation = new TimeSpan(0);
 
        [ObservableProperty]
        private bool isFiller = false;

        public List<ClockItemCategoryTagDTO>? Tags { get; set; } = new();

        public static ManageClockCategoryModel FromDto(ClockItemCategoryDTO dto)
        {
            return new ManageClockCategoryModel
            {
                FromReleaseDate = dto.MinReleaseDate,
                ToReleaseDate = dto.MaxReleaseDate,
                MinDuration = dto.MinDuration.HasValue ? dto.MinDuration : new TimeSpan(0, 0, 0),
                MaxDuration = dto.MaxDuration.HasValue ? dto.MaxDuration : new TimeSpan(0, 999, 0),
                ArtistSeparation = new TimeSpan(0,dto.ArtistSeparation.GetValueOrDefault(),0),
                TitleSeparation = new TimeSpan(0,dto.TitleSeparation.GetValueOrDefault(),0),
                TrackSeparation = new TimeSpan(0,dto.TrackSeparation.GetValueOrDefault(),0),
                IsFiller = dto.IsFiller,
                Tags = dto.Tags,
            };
        }

        public static ClockItemCategoryDTO ToDto(ManageClockCategoryModel model)
        {
            //Convert the model to dto
            return new ClockItemCategoryDTO
            {
                MinReleaseDate = model.FromReleaseDate,
                MaxReleaseDate = model.ToReleaseDate,
                MinDuration = model.EnforceDurationLimits ? model.MinDuration.GetValueOrDefault() : null,
                MaxDuration = model.EnforceDurationLimits ? model.MaxDuration.GetValueOrDefault() : null,
                ArtistSeparation = model.ArtistSeparation.HasValue && model.ArtistSeparation.Value.Minutes > 0 
                    ? (int)model.ArtistSeparation.Value.Minutes : null,
                TitleSeparation = model.TitleSeparation.HasValue  && model.TitleSeparation.Value.Minutes > 0
                    ? (int)model.TitleSeparation.Value.Minutes : null,
                TrackSeparation = model.TrackSeparation.HasValue && model.TrackSeparation.Value.Minutes > 0
                    ? (int)model.TrackSeparation.Value.Minutes: null,
                IsFiller = model.IsFiller,
                Tags = model.Tags,

            };
        }

    }
}
