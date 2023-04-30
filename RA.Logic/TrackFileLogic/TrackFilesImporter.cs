using RA.Database;
using RA.DTO;
using RA.Logic.TrackFileLogic.Interfaces;
using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Logic.TrackFileLogic
{
    public class TrackFilesImporter : ITrackFilesImporter
    {
        public int Import(List<ProcessingTrack> processingTracks)
        {
            Debug.WriteLine("Importing tracks...");
            int numberOfTracks = 0;
            using(var db = new AppDbContext())
            {
                var tracks = processingTracks.Select(t => TrackDto.ToEntity(t.TrackDto)).ToList();

                // Attach the related Category entities to the DbContext
                foreach (var track in tracks)
                {
                    if (track.Categories.Any())
                    {
                        var category = track.Categories.First();
                        var localCategory = db.AttachOrGetTrackedEntity<Category>(category);
                        if (localCategory != category)
                        {
                            track.Categories.Remove(category);
                            track.Categories.Add(localCategory);
                        }
                    }
                }

                db.AddRange(tracks);
                db.SaveChanges();
                numberOfTracks = tracks.Count;
            }
            return numberOfTracks;
        }
    }
}
