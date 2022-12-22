using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Database.Config
{
    public class ArtistTrackConfig : IEntityTypeConfiguration<ArtistTrack>
    {
        public void Configure(EntityTypeBuilder<ArtistTrack> builder)
        {
            builder.HasKey(ta => new { ta.ArtistId, ta.TrackId });

            builder.HasOne(ta => ta.Artist)
               .WithMany(a => a.ArtistTracks)
               .HasForeignKey(ta => ta.ArtistId);

            builder.HasOne(ta => ta.Track)
                .WithMany(t => t.TrackArtists)
                .HasForeignKey(ta => ta.TrackId);

            
        }
    }
}
