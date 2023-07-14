using CommunityToolkit.Mvvm.ComponentModel;
using RA.Database.Models.Enums;
using RA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.Planner.ViewModels.MainContent.Models
{
    public partial class PlaylistItemModel : ObservableObject
    {
        public int Id { get; set; }

        public int PlaylistId { get; set; }

        public string? Label { get; set; }

        [ObservableProperty]
        private int index;

        [ObservableProperty]
        public DateTime eTA;

        [ObservableProperty]
        public double length;

        [ObservableProperty]
        public string itemDetails = string.Empty;
        public TrackListingDTO? Track { get; set; }

        public PlaylistItemModel()
        {
            
        }

        public void UpdateItemDetails()
        {
            if(Track != null)
            {
                if (Track.Artists == String.Empty)
                {
                    ItemDetails = $"{Track.Title}";
                }
                else
                {
                    ItemDetails = $"{Track.Artists} - {Track.Title}";
                }
            }
            else
            {
                if (Label != null) ItemDetails = Label;
                else
                ItemDetails = string.Empty;
            }
        }

        public static PlaylistItemModel FromDTO(PlaylistItemDTO dto)
        {
            return new PlaylistItemModel
            {
                Id = dto.Id,
                ETA = dto.ETA,
                Length = dto.Length,
                PlaylistId = dto.PlaylistId,
                Track = dto.Track,
                Label = dto.Label,
            };
        }

    }
}
