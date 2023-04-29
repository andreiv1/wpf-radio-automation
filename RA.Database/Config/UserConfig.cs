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
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Username).IsRequired()
                .HasMaxLength(60);
            builder.Property(u => u.Password).IsRequired()
                .HasMaxLength(60);
            builder.Property(u => u.FullName)
                .HasMaxLength(100);
        }
    }
}
