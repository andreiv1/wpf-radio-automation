using CommunityToolkit.Mvvm.ComponentModel;
using RA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.MediaLibrary.Models
{
    public partial class ArtistTrackModel : ObservableObject
    {
        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private string artists;

        public static ArtistTrackModel FromDto(TrackListingDTO dto)
        {
            return new ArtistTrackModel
            {
                Artists = dto.Artists,
                Title = dto.Title,
            };
        }
    }
}
