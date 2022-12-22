using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RA.Database.Models;

namespace RA.Database.Config
{
    public class TrackHistoryConfig : IEntityTypeConfiguration<TrackHistory>
    {
        public void Configure(EntityTypeBuilder<TrackHistory> builder)
        {
            builder.HasOne(th => th.Track)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
