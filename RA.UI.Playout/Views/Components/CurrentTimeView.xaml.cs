using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RA.UI.Playout.Views.Components
{
    public partial class CurrentTimeView : UserControl, INotifyPropertyChanged
    {
        private DateTime currentTime = DateTime.Now;
        private readonly CancellationTokenSource cts = new CancellationTokenSource();

        public DateTime CurrentTime
        {
            get => currentTime;
            set
            {
                currentTime = value;
                OnPropertyChanged(nameof(CurrentTime));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CurrentTimeView()
        {
            InitializeComponent();
            DataContext = this;
            _ = InitializeClockAsync();
        }

        private async Task InitializeClockAsync()
        {
            while (!cts.Token.IsCancellationRequested)
            {
                await Task.Delay(500, cts.Token);
                CurrentTime = DateTime.Now;
            }
        }

        // Call this method to stop the timer task when you no longer need it
        public void StopClock()
        {
            cts.Cancel();
        }
    }

}
