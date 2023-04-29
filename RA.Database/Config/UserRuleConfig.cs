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
    public class UserRuleConfig : IEntityTypeConfiguration<UserRule>
    {
        public void Configure(EntityTypeBuilder<UserRule> builder)
        {
            builder.Property(ur => ur.RuleName)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
