using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RA.UI.Core.Services
{
    public interface IDispatcherService
    {
        void InvokeOnUIThread(Action action);
        Task InvokeOnUIThreadAsync(Action action);
    }
    public class WpfDispatcherService : IDispatcherService
    {
        public void InvokeOnUIThread(Action action)
        {
            Application.Current.Dispatcher.Invoke(action);
        }

        public async Task InvokeOnUIThreadAsync(Action action)
        {
            await Application.Current.Dispatcher.InvokeAsync(action);
        }
    }
}
