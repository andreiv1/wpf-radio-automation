using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RA.UI.Core.Services
{
    public interface IMessageBoxService
    {
        void ShowError(string message);
        void ShowWarning(string message);
        void ShowInfo(string message);
        void ShowYesNoInfo(string message, Action actionYes, Action actionNo);
    }
    public class MessageBoxService : IMessageBoxService
    {
        public void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ShowWarning(string message)
        {
            MessageBox.Show(message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public void ShowInfo(string message)
        {
            MessageBox.Show(message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ShowYesNoInfo(string message, Action actionYes, Action actionNo)
        {
            var result = MessageBox.Show(message, "Information", MessageBoxButton.YesNo);
            if(result == MessageBoxResult.Yes)
            {
                actionYes();
            } else if(result == MessageBoxResult.No)
            {
                actionNo();
            }
        }
    }
}
