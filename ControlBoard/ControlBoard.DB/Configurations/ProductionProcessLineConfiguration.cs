using ControlBoard.DB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlBoard.DB.Configurations
{
    public class ProductionProcessLineConfiguration : IEntityTypeConfiguration<ProductionProcessLine>
    {
        public void Configure(EntityTypeBuilder<ProductionProcessLine> builder)
        {
            builder.ToTable("areas").HasQueryFilter(f => !f.IsDeleted);
            builder.Property(p => p.Id).HasColumnName("id");
            builder.Property(p => p.OrderNum).HasMaxLength(3).HasColumnName("order_num");
            builder.Property(p => p.SkipTs).HasColumnName("skip_ts");
            builder.Property(p => p.MultiScanning).HasColumnName("multi_scanning");
            builder.Property(p => p.CheckSequence).HasColumnName("check_sequence");
            builder.Property(p => p.Created).HasColumnName("created");
            builder.Property(p => p.LastUpdated).HasColumnName("last_updated");
            builder.Property(p => p.IsDeleted).HasDefaultValue(false).HasColumnName("is_deleted");
        }
    }
}