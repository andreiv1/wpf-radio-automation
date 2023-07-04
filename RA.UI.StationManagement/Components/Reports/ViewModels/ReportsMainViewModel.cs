using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.ThreadedComments;
using RA.DAL;
using RA.Database.Models.Enums;
using RA.DTO;
using RA.UI.Core.Services;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using Syncfusion.XlsIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;
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
        private readonly IMessageBoxService messageBoxService;
        private readonly ITrackHistoryService trackHistoryService;

        [ObservableProperty]
        private DateTime startDate;

        private DateTime actualStartDate;

        [ObservableProperty] 
        private DateTime endDate = DateTime.Today.Date;

        private DateTime actualEndDate;
        public string[] TrackTypes => Enum.GetNames(typeof(TrackType));

        private string[] actualTrackTypes;

        [ObservableProperty]
        private string reportTitle = "No report generated.";

        public ObservableCollection<string> SelectedTrackTypes { get; set; } = new();

        public ObservableCollection<TrackHistoryListingDTO> TrackHistoryReport { get; set; } = new();

        public ReportsMainViewModel(IDispatcherService dispatcherService,
                                    IWindowService windowService,
                                    IMessageBoxService messageBoxService,
                                    ITrackHistoryService trackHistoryService)
        {
            InitSelectedTrackTypes();
            StartDate = EndDate.AddDays(-31);
            this.dispatcherService = dispatcherService;
            this.windowService = windowService;
            this.messageBoxService = messageBoxService;
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
            //if(EndDate <= StartDate)
            //{
            //    messageBoxService.ShowError("The end date must be greater than start date.");
            //    return;
            //}
    
            await dispatcherService.InvokeOnUIThreadAsync(() => TrackHistoryReport.Clear());
            actualStartDate = StartDate;
            actualEndDate = EndDate;
            actualTrackTypes = SelectedTrackTypes.ToArray();
            List<TrackType> convertedTrackTypes = SelectedTrackTypes.Select(x => (TrackType)Enum.Parse(typeof(TrackType), x)).ToList();
            
            var report = await trackHistoryService.GetHistoryBetween(StartDate, EndDate, convertedTrackTypes);
            foreach(var item in report)
            {
                await dispatcherService.InvokeOnUIThreadAsync(() => TrackHistoryReport.Add(item));
            }
            if (actualStartDate != actualEndDate)
            {
                ReportTitle = $"Track history report from {actualStartDate.ToString("dd.MM.yyyy")} to {actualEndDate.ToString("dd.MM.yyyy")}";
            }
            else
            {
                ReportTitle = $"Track history report for {actualStartDate.ToString("dd.MM.yyyy")}";
            }
            ExportExcelFileCommand.NotifyCanExecuteChanged();
            ExportCsvFileCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(CanExportAnyFile))]
        private async Task ExportExcelFile()
        {
            await Task.Run(() =>
            {
                using var package = new ExcelPackage();
                var worksheet = package.Workbook.Worksheets.Add($"TrackHistory");

                var mergeRange = worksheet.Cells["A1:E1"];
                mergeRange.Merge = true;
                mergeRange.Value = ReportTitle;
                mergeRange.Style.Font.Size = 17;
                mergeRange.Style.Font.Bold = true;
                mergeRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                mergeRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                var mergeRange2 = worksheet.Cells["A2:E2"];
                mergeRange2.Merge = true;
                mergeRange2.Value = $"Selected track types: {string.Join(", ",actualTrackTypes)}";
                mergeRange2.Style.Font.Bold = true;
                mergeRange2.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                mergeRange2.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);


                worksheet.Cells["A3"].Value = "Date played";
                worksheet.Cells["B3"].Value = "Type";
                worksheet.Cells["C3"].Value = "Artists";
                worksheet.Cells["D3"].Value = "Title";
                worksheet.Cells["E3"].Value = "ISRC";

                

                var headerStyle = worksheet.Cells["A3:E3"].Style;
                headerStyle.Font.Bold = true;
                headerStyle.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                headerStyle.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                int row = 4;
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

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                saveFileDialog.InitialDirectory = desktopPath;
                saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
                if (actualStartDate != actualEndDate)
                {
                    saveFileDialog.FileName = $"track_report_{actualStartDate.ToString("dd.MM.yyyy")}_{actualEndDate.ToString("dd.MM.yyyy")}.xlsx";
                }
                else
                {
                    saveFileDialog.FileName = $"track_report_{actualStartDate.ToString("dd.MM.yyyy")}.xlsx";
                }
                if (saveFileDialog.ShowDialog() == true)
                {
                    if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                    {
                        try
                        {
                            package.SaveAs(saveFileDialog.FileName);
                        }
                        catch(Exception )
                        {
                            messageBoxService.ShowError($"File couldn't be saved.");
                        }

                        try
                        {
                            messageBoxService.ShowYesNoInfo(title: "File generated",
                                message: "Would you like to open the generated Excel file?",
                                actionYes: () => {
                                    ProcessStartInfo processStartInfo = new ProcessStartInfo(saveFileDialog.FileName)
                                    {
                                        UseShellExecute = true
                                    };
                                    Process.Start(processStartInfo);
                                },
                                actionNo: () => { });
                        } catch(Exception)
                        {
                            messageBoxService.ShowWarning($"An error occured while opening the file: {saveFileDialog.FileName}");
                        }
                        
                    }

                    
                }
            });
           

        }

        [RelayCommand(CanExecute = nameof(CanExportAnyFile))]
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

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                saveFileDialog.InitialDirectory = desktopPath;
                saveFileDialog.Filter = "Csv files (*.csv)|*.csv";
                if (actualStartDate != actualEndDate)
                {
                    saveFileDialog.FileName = $"track_report_{actualStartDate.ToString("dd.MM.yyyy")}_{actualEndDate.ToString("dd.MM.yyyy")}.csv";
                }
                else
                {
                    saveFileDialog.FileName = $"track_report_{actualStartDate.ToString("dd.MM.yyyy")}.csv";
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                    {
                        try
                        {
                            File.WriteAllText(saveFileDialog.FileName, csvBuilder.ToString());
                        }
                        catch (Exception)
                        {
                            messageBoxService.ShowError($"File couldn't be saved.");
                        }

                        try
                        {
                            messageBoxService.ShowYesNoInfo(title: "File generated",
                                message: "Would you like to open the generated CSV file?",
                                actionYes: () => {
                                    ProcessStartInfo processStartInfo = new ProcessStartInfo(saveFileDialog.FileName)
                                    {
                                        UseShellExecute = true
                                    };
                                    Process.Start(processStartInfo);
                                },
                                actionNo: () => { });
                        }
                        catch (Exception)
                        {
                            messageBoxService.ShowWarning($"An error occured while opening the file: {saveFileDialog.FileName}");
                        }
                    }

                    
                }
            });
        }

        private bool CanExportAnyFile()
        {
            return TrackHistoryReport.Count > 0;
        }
    }
}
