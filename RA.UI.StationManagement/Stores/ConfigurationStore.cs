using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Stores
{
    public class ConfigurationStore
    {
        public string AudioPath { get; private set; }
        public string ImagePath { get; private set; }

        public int DefaultArtistSeparation { get; private set; } = 30;
        public int DefaultTitleSeparation { get; private set; } = 60;
        public int DefaultTrackSeparation { get; private set; } = 120;

        public ConfigurationStore(IConfiguration configuration)
        {
            if(configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }   
            ImagePath = configuration["AppSettings:ImagePath"];
            AudioPath = configuration["AppSettings:AudioPath"];

            if (string.IsNullOrEmpty(ImagePath) || string.IsNullOrEmpty(AudioPath))
            {
                throw new Exception("Required configuration values are missing.");
            }

            if (!int.TryParse(configuration["AppSettings:DefaultArtistSeparation"], out int artistSeparation) ||
                !int.TryParse(configuration["AppSettings:DefaultTitleSeparation"], out int titleSeparation) ||
                !int.TryParse(configuration["AppSettings:DefaultTrackSeparation"], out int trackSeparation))
            {
                throw new Exception("Invalid configuration values for default separations.");
            }

            DefaultArtistSeparation = artistSeparation;
            DefaultTitleSeparation = titleSeparation;
            DefaultTrackSeparation = trackSeparation;
        }

        public string GetFullImagePath(string image)
        {
            return Path.Combine(ImagePath, image);
        }

        public static string GetDefaultImagePath()
        {
            return "pack://application:,,,/RA.UI.Core;component/Resources/Images/track_default_image.png";
        }
    }
}
