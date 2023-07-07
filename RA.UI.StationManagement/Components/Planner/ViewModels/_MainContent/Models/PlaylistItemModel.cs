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
    public class PlaylistItemModel : ObservableObject
    {
        public int Index { get; set; }
        public int Id { get; set; }
        public DateTime ETA { get; set; }
        public double Length { get; set; }
        public int PlaylistId { get; set; }

        public TrackListingDTO? Track { get; set; }

        public String ItemDetails
        {
            get
            {
                if(Track != null)
                {
                    if(Track.Artists == String.Empty)
                    {
                        return $"{Track.Title}";
                    }
                    return $"{Track.Artists} - {Track.Title}";
                }

                return string.Empty;
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
            };
        }

    }
}
