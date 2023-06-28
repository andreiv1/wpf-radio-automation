using RA.DAL;
using RA.DTO;
using RA.UI.Core.Services;
using RA.UI.Core.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RA.UI.Playout.ViewModels.Components
{
    public partial class HistoryViewModel : ViewModelBase
    {
        private readonly ITrackHistoryService trackHistoryService;
        private readonly IDispatcherService dispatcherService;

        private const int itemsToKeep = 500;

        public MainViewModel? MainVm { get; set; }

        public ObservableCollection<TrackHistoryListingDTO> TrackHistory { get; set; } = new();

        public HistoryViewModel(ITrackHistoryService trackHistoryService, IDispatcherService dispatcherService)
        {
            this.trackHistoryService = trackHistoryService;
            this.dispatcherService = dispatcherService;
            _ = LoadPreviousHistory();
        }


        public async Task LoadPreviousHistory()
        {
            var date = DateTime.Now.Date.AddDays(-1);
            var result = await trackHistoryService.RetrieveTrackHistory(date);
            foreach(var item in result)
            {
                await dispatcherService.InvokeOnUIThreadAsync(() => TrackHistory.Add(item));
            }
        }

        public async Task AddItem(DateTime date)
        {
            var result = await trackHistoryService.RetrieveItem(date);
            if (result != null)
            {
                await dispatcherService.InvokeOnUIThreadAsync(() =>
                {
                    TrackHistory.Insert(0, result);
                    if(TrackHistory.Count > itemsToKeep)
                    {
                        TrackHistory.RemoveAt(TrackHistory.Count - 1);
                    }
                });
            }
                
        }
    }
}
