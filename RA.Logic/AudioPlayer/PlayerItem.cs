using RA.DTO;
using RA.Logic.AudioPlayer.Interfaces;
using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RA.Database.Models.Enums;

namespace RA.Logic.AudioPlayer
{
    public class PlayerItem : IPlayerItem, INotifyPropertyChanged
    {
        
        public string FilePath => throw new NotImplementedException();

        public TimeSpan Duration => throw new NotImplementedException();

        public DateTime ETA { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string? Artists => throw new NotImplementedException();

        public string Title => throw new NotImplementedException();

        public string? TrackType => throw new NotImplementedException();

        public string ImagePath => throw new NotImplementedException();

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
