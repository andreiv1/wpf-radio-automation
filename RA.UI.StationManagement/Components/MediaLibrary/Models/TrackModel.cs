using CommunityToolkit.Mvvm.ComponentModel;
using RA.Database.Models.Enums;
using RA.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
        private TimeSpan duration = TimeSpan.Zero;

        [ObservableProperty]
        private double startCue = 0;

        [ObservableProperty]
        private double endCue;

        [ObservableProperty]
        private double nextCue;

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
        public ObservableCollection<TrackTagDTO>? Tags { get; set; }
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
                Duration = TimeSpan.FromSeconds(dto.Duration),
                StartCue = dto.StartCue ?? 0,
                NextCue = dto.NextCue ?? dto.Duration,
                EndCue = dto.EndCue ?? dto.Duration,
                ReleaseDate = dto.ReleaseDate,
                Album = dto.Album ?? "",
                Comments = dto.Comments ?? "",
                FilePath = dto.FilePath ?? "",
                ImageName = dto.ImageName ?? "",
                Bpm = dto.Bpm,
                ISRC = dto?.ISRC ?? "",
                DateAdded = dto.DateAdded,
                DateModified = dto.DateModified,
                DateDeleted = dto.DateDeleted,
                Artists = dto.Artists != null ? new ObservableCollection<TrackArtistDTO>(dto.Artists) : null,
                Categories = dto.Categories != null ? new ObservableCollection<TrackCategoryDTO>(dto.Categories) : null,
                Tags = dto.Tags != null ? new ObservableCollection<TrackTagDTO>(dto.Tags) : null,
            };
        }

        public static TrackDTO ToDto(TrackModel model)
        {
            return new TrackDTO
            {
                Id = model.Id,
                Type = model.Type,
                Status = model.Status,
                Duration = model.Duration.TotalSeconds,
                Title = model.Title,
                StartCue = model.StartCue == 0 ? null : model.StartCue,
                NextCue = model.NextCue == model.Duration.TotalSeconds ? null : model.NextCue,
                EndCue = model.EndCue == model.Duration.TotalSeconds ? null : model.EndCue,
                ReleaseDate = model.ReleaseDate,
                Album = model.Album,
                Comments = model.Comments,
                FilePath = model.FilePath,
                ImageName = model.ImageName,
                Bpm = model.Bpm,
                ISRC = model.ISRC,
                Artists = model.Artists?.ToList(),
                Categories = model?.Categories?.ToList(),
                Tags = model.Tags.ToList(),
                
            };
        }
    }
}
