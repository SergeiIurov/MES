using ControlBoard.DB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlBoard.DB.Configurations
{
    public class AreaConfiguration : IEntityTypeConfiguration<Area>
    {
        public void Configure(EntityTypeBuilder<Area> builder)
        {
            builder.ToTable("Areas").HasQueryFilter(f => !f.IsDeleted);
            builder.Property(p => p.IsDeleted).HasDefaultValue(false).IsRequired();
            builder.Property(p => p.Name).HasMaxLength(150).IsRequired();
            builder.Property(p => p.Created).IsRequired();
            builder.Property(p => p.LastUpdated).IsRequired();
        }
    }
}
