using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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
    /// <summary>
    /// Interaction logic for CurrentTimeView.xaml
    /// </summary>
    public partial class CurrentTimeView : UserControl, INotifyPropertyChanged
    {
        private DateTime currentTime = DateTime.Now;

        public DateTime CurrentTime
        {
            get => currentTime;
            set
            {
                currentTime = value;
                OnPropertyChanged(nameof(CurrentTime));
            }
        }

        private System.Timers.Timer clockTimer;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CurrentTimeView()
        {
            InitializeComponent();
            InitializeClock();
            DataContext = this;
            
        }

        private void InitializeClock()
        {
            clockTimer = new System.Timers.Timer(1000);
            clockTimer.Elapsed += ClockTimer_Elapsed;
            clockTimer.AutoReset = true;
            clockTimer.Start();
        }

        private void ClockTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            CurrentTime = DateTime.Now;
        }
    }
}
