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
    public class PlaylistItemConfig : IEntityTypeConfiguration<PlaylistItem>
    {
        public void Configure(EntityTypeBuilder<PlaylistItem> builder)
        {
            builder.HasOne(pi => pi.BaseClockItem)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(pi => pi.BaseTemplate)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(pi => pi.ParentPlaylistItem)
                .WithMany()
                .HasForeignKey(pi => pi.ParentPlaylistItemId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
