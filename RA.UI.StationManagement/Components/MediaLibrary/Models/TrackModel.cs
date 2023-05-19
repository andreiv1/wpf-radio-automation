using CommunityToolkit.Mvvm.ComponentModel;
using RA.Database.Models.Enums;
using RA.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.MediaLibrary.Models
{
    public partial class TrackModel : ObservableObject
    {
        public int Id { get; private set; }

        [ObservableProperty]
        private TrackType type;

        [ObservableProperty]
        private TrackStatus status;

        [ObservableProperty]
        private string title = string.Empty;

        [ObservableProperty]
        private double duration = 0;

        [ObservableProperty]
        private DateTime? releaseDate;

        [ObservableProperty]
        private string album = string.Empty;

        [ObservableProperty]
        private string comments = string.Empty;

        [ObservableProperty]
        private string lyrics = string.Empty;

        [ObservableProperty]
        private string filePath = string.Empty;

        [ObservableProperty]
        private string imageName = string.Empty;

        [ObservableProperty]
        private int? bpm;

        [ObservableProperty]
        private string iSRC = string.Empty;

        [ObservableProperty]
        private DateTime dateAdded;

        [ObservableProperty]
        private DateTime? dateModified;

        [ObservableProperty]
        private DateTime? dateDeleted;
        public ObservableCollection<TrackArtistDTO>? Artists { get; set; }
        public ObservableCollection<TrackCategoryDTO>? Categories { get; set; }
        public String ArtistsText
        {
            get
            {
                if (Artists is not null)
                {
                    return String.Join(" / ", Artists.Select(a => a.ArtistName));
                }
                return "";
            }
        }
        public static List<string> TrackTypes => Enum.GetNames(typeof(TrackType)).ToList();
        public static List<string> TrackStatuses => Enum.GetNames(typeof(TrackStatus)).ToList();
        public static TrackModel FromDto(TrackDTO dto)
        {
            
            return new TrackModel()
            {
                Id = dto.Id,
                Type = dto.Type,
                Status = dto.Status,
                Title = dto.Title ?? "",
                Duration = dto.Duration,
                ReleaseDate = dto.ReleaseDate,
                Album = dto.Album ?? "",
                Comments = dto.Comments ?? "",
                Lyrics = dto.Lyrics ?? "",
                FilePath = dto.FilePath ?? "",
                ImageName = dto.ImageName ?? "",
                Bpm = dto.Bpm,
                ISRC = dto.ISRC,
                DateAdded = dto.DateAdded,
                DateModified = dto.DateModified,
                DateDeleted = dto.DateDeleted,
                Artists = dto.Artists != null ? new ObservableCollection<TrackArtistDTO>(dto.Artists) : null,
                Categories = dto.Categories != null ? new ObservableCollection<TrackCategoryDTO>(dto.Categories) : null,
            };
        }
    }
}
