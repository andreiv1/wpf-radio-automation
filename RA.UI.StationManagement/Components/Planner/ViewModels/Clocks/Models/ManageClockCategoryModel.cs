using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.Planner.ViewModels.Clocks.Models
{
    public partial class ManageClockCategoryModel : ObservableObject
    {
        #region Release Date
        [ObservableProperty]
        private DateTime? fromReleaseDate;

        [ObservableProperty] 
        private DateTime? toReleaseDate;

        [ObservableProperty]
        private bool excludeWithoutReleaseDate = false;
        #endregion

        #region Bpm

        [ObservableProperty]
        private int? minBpm;

        [ObservableProperty]
        private int? maxBpm;

        [ObservableProperty]
        private bool excludeWithoutBpm = false;

        #endregion

        #region Duration

        [ObservableProperty]
        private TimeSpan? minDuration;

        [ObservableProperty]
        private TimeSpan? maxDuration;

        [ObservableProperty]
        private bool enforceDurationLimits = false;

        #endregion

        #region Separation
        [ObservableProperty]
        private TimeSpan? artistSeparation = new TimeSpan(0);

        [ObservableProperty]
        private TimeSpan? titleSeparation = new TimeSpan(0);

        [ObservableProperty]
        private TimeSpan? trackSeparation = new TimeSpan(0);
        #endregion
        [ObservableProperty]
        private bool isFiller = false;

      
        //TODO: tags

    }
}
