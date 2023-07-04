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
            };
        }

    }
}
