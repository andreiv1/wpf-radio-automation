using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace RA.UI.Core.Services
{
    public interface IFolderBrowserDialogService
    {
        string SelectedPath { get; }
        DialogResult ShowDialog();
    }
    public class FolderBrowserDialogService : IFolderBrowserDialogService
    {
        public string SelectedPath => dialog.SelectedPath;

        private readonly FolderBrowserDialog dialog;

        public FolderBrowserDialogService()
        {
            dialog = new FolderBrowserDialog(); 
        }

        public DialogResult ShowDialog() => dialog.ShowDialog();
    }
}
