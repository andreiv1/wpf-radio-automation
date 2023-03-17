using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RA.UI.Core.Services
{
    public interface IFileBrowserDialogService
    {
        string SelectedPath { get; }
        string DefaultExt { get; set; }
        bool AddExtension { get; set; }
        string Filter { get; set; }

        DialogResult ShowDialog();
    }
    public class FileBrowserDialogService : IFileBrowserDialogService, IDisposable
    {
        public string SelectedPath => dialog.FileName;
        public string DefaultExt { get => dialog.DefaultExt; set => dialog.DefaultExt = value; }
        public bool AddExtension { get => dialog.AddExtension; set => dialog.AddExtension = value; }
        public string Filter { get => dialog.Filter; set => dialog.Filter = value; }

        private readonly OpenFileDialog dialog;

        public FileBrowserDialogService()
        {
            dialog = new OpenFileDialog();
        }

        public DialogResult ShowDialog()
        {
            return dialog.ShowDialog();
        }

        public void Dispose()
        {
            dialog.Dispose();
        }
    }
}
