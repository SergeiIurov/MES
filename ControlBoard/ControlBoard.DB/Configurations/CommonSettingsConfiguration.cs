using ControlBoard.DB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlBoard.DB.Configurations
{
    public class CommonSettingsConfiguration : IEntityTypeConfiguration<CommonSettings>
    {
        public void Configure(EntityTypeBuilder<CommonSettings> builder)
        {
            builder.ToTable("common_settings").HasQueryFilter(f => !f.IsDeleted);
            builder.Property(p => p.Id).HasColumnName("id");
            builder.Property(p => p.LineCount).HasColumnName("line_count").HasDefaultValue(1);
            builder.Property(p => p.Created).HasColumnName("created");
            builder.Property(p => p.LastUpdated).HasColumnName("last_updated");
            builder.Property(p => p.IsDeleted).HasDefaultValue(false).HasColumnName("is_deleted");
           }
    }
}
