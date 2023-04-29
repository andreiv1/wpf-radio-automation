using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace RA.Database.Config
{
    public class PlaylistConfig : IEntityTypeConfiguration<Playlist>
    {
        public void Configure(EntityTypeBuilder<Playlist> builder)
        {
            builder.HasMany(p => p.PlaylistItems)
               .WithOne(pi => pi.Playlist)
               .HasForeignKey(pi => pi.PlaylistId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
