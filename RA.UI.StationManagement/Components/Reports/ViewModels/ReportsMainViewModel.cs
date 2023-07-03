using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OfficeOpenXml;
using OfficeOpenXml.ThreadedComments;
using RA.DAL;
using RA.Database.Models.Enums;
using RA.DTO;
using RA.UI.Core.Services;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace RA.UI.StationManagement.Components.Reports.ViewModels
{
    public partial class ReportsMainViewModel : ViewModelBase
    {
        private readonly IDispatcherService dispatcherService;
        private readonly IWindowService windowService;
        private readonly ITrackHistoryService trackHistoryService;

        [ObservableProperty]
        private DateTime startDate = DateTime.Today.Date;

        private DateTime actualStartDate;

        [ObservableProperty] 
        private DateTime endDate;

        private DateTime actualEndDate;
        public string[] TrackTypes => Enum.GetNames(typeof(TrackType));

        public ObservableCollection<string> SelectedTrackTypes { get; set; } = new();

        public ObservableCollection<TrackHistoryListingDTO> TrackHistoryReport { get; set; } = new();

        public ReportsMainViewModel(IDispatcherService dispatcherService,
                                    IWindowService windowService,
                                    ITrackHistoryService trackHistoryService)
        {
            InitSelectedTrackTypes();
            EndDate = StartDate.AddDays(31);
            this.dispatcherService = dispatcherService;
            this.windowService = windowService;
            this.trackHistoryService = trackHistoryService;
        }

        private void InitSelectedTrackTypes()
        {
            foreach (var trackType in TrackTypes)
            {
                SelectedTrackTypes.Add(trackType);
            }
        }

        [RelayCommand]
        private async Task GenerateReport()
        {
            await dispatcherService.InvokeOnUIThreadAsync(() => TrackHistoryReport.Clear());
            actualStartDate = StartDate;
            actualEndDate = EndDate;    
            List<TrackType> convertedTrackTypes = SelectedTrackTypes.Select(x => (TrackType)Enum.Parse(typeof(TrackType), x)).ToList();
            
            var report = await trackHistoryService.GetHistoryBetween(StartDate, EndDate, convertedTrackTypes);
            foreach(var item in report)
            {
                await dispatcherService.InvokeOnUIThreadAsync(() => TrackHistoryReport.Add(item));
            }
        }

        [RelayCommand]
        private async Task ExportExcelFile()
        {
            await Task.Run(() =>
            {
                using var package = new ExcelPackage();
                var worksheet = package.Workbook.Worksheets.Add($"TrackHistory");

                var mergeRange = worksheet.Cells["A1:E1"];
                mergeRange.Merge = true;
                mergeRange.Value = $"Track history report from {actualStartDate.ToString("dd.MM.yyyy")} to {actualEndDate.ToString("dd.MM.yyyy")}";
                mergeRange.Style.Font.Size = 15;
                mergeRange.Style.Font.Bold = true;
                mergeRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                mergeRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);


                worksheet.Cells["A2"].Value = "Date played";
                worksheet.Cells["B2"].Value = "Type";
                worksheet.Cells["C2"].Value = "Artists";
                worksheet.Cells["D2"].Value = "Title";
                worksheet.Cells["E2"].Value = "ISRC";

                

                var headerStyle = worksheet.Cells["A2:E2"].Style;
                headerStyle.Font.Bold = true;
                headerStyle.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                headerStyle.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                int row = 3;
                foreach (var track in TrackHistoryReport)
                {
                    worksheet.Cells[row, 1].Value = track.DatePlayed.ToString("G");
                    worksheet.Cells[row, 2].Value = track.TrackType.ToString();
                    worksheet.Cells[row, 3].Value = track.Artists;
                    worksheet.Cells[row, 4].Value = track.Title;
                    worksheet.Cells[row, 5].Value = track.ISRC;
                    row++;
                }

                worksheet.Column(1).AutoFit();
                worksheet.Column(2).AutoFit();
                worksheet.Column(3).AutoFit();
                worksheet.Column(4).AutoFit();
                worksheet.Column(5).AutoFit();

                package.SaveAs(new FileInfo(@"C:\Users\Andrei\Desktop\report.xlsx"));
            });
           

        }

        [RelayCommand]
        private async Task ExportCsvFile()
        {
            await Task.Run(() =>
            {
                var csvBuilder = new StringBuilder();
                csvBuilder.AppendLine("Date played,Type,Artists,Title,ISRC");

                foreach (var track in TrackHistoryReport)
                {
                    csvBuilder.AppendLine($"{track.DatePlayed.ToString("G")},{track.TrackType.ToString()},{track.Artists},{track.Title},{track.ISRC}");
                }
                File.WriteAllText(@"C:\Users\Andrei\Desktop\report.csv", csvBuilder.ToString());
            });
        }
    }
}
