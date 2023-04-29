using CommunityToolkit.Mvvm.ComponentModel;
using RA.Database;
using RA.Dto;
using RA.UI.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.Playout.ViewModels.Controls
{
    public partial class MediaItemsViewModel : PaginationViewModel<TrackListDto>
    {
        [ObservableProperty]
        private TrackListDto? selectedTrack;

        [ObservableProperty]
        private String searchQuery = "";
        public MainViewModel MainVm { get; set; }
        public MediaItemsViewModel() : base()
        {
            LoadItems();
        }

        public void LoadItems()
        {
            _ = LoadItemsAsync(isFirstPage: true);
        }

        protected override IQueryable<TrackListDto> GetItemsQuery(AppDbContext db)
        {
            return db.GetTracks(SearchQuery).Select(t => TrackListDto.FromEntity(t));
        }
    }
}
