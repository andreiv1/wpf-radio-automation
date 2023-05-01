using CommunityToolkit.Mvvm.ComponentModel;
using RA.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.MediaLibrary.Models
{
    public partial class ArtistModel : ObservableObject
    {
        public int Id { get; private set; } = 0;
        [ObservableProperty]
        private string name = "";
        public ObservableCollection<ArtistTrackModel>? Tracks { get; set; }
        public static ArtistModel FromDto(ArtistDto dto)
        {
            return new ArtistModel
            {
                Id = dto.Id,
                name = dto.Name,
            };
        }
    }
}
