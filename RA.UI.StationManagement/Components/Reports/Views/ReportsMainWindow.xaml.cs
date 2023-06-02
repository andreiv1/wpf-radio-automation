using Microsoft.Win32;
using RA.UI.Core;
using Syncfusion.UI.Xaml.Grid.Converter;
using Syncfusion.XlsIO;
using System;
using System.Windows;
using System.Windows.Controls;

namespace RA.UI.StationManagement.Components.Reports.Views
{
    public partial class ReportsMainWindow : RAWindow
    {
        public ReportsMainWindow()
        {
            InitializeComponent();
        }

        private void btnExportCsv_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var options = new ExcelExportingOptions();
            options.ExcelVersion = ExcelVersion.Excel2013;
            if (reportsDataGrid != null && reportsDataGrid.View != null)
            {
                var excelEngine = reportsDataGrid.ExportToExcel(reportsDataGrid.View, options);
                var workBook = excelEngine.Excel.Workbooks[0];
                workBook.SaveAs("Sample.csv", ",");
            }
            else
            {
                // Handle error, such as displaying an error message to the user
                MessageBox.Show("Error: reportsDataGrid or its View is null.");
            }


        }

        private void btnExportXls_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var options = new ExcelExportingOptions();
            options.ExcelVersion = Syncfusion.XlsIO.ExcelVersion.Excel2013;
            var excelEngine = reportsDataGrid.ExportToExcel(reportsDataGrid.View, options);
            var workBook = excelEngine.Excel.Workbooks[0];
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            saveFileDialog.InitialDirectory = desktopPath;
            saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";

            if (saveFileDialog.ShowDialog() == true)
            {
                workBook.SaveAs(saveFileDialog.FileName);
            }
                
        }
    }
}
