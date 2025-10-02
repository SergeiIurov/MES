using ControlBoard.DB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlBoard.DB.Configurations
{
    public class ScanningPointConfiguration : IEntityTypeConfiguration<ScanningPoint>
    {
        public void Configure(EntityTypeBuilder<ScanningPoint> builder)
        {
            builder.ToTable("scanning_points").HasQueryFilter(f => !f.IsDeleted);
            builder.Property(p => p.Id).HasColumnName("id");
            builder.Property(p => p.Name).HasMaxLength(150).HasColumnName("name").HasDefaultValue("");
            builder.Property(p => p.OrderNum).HasMaxLength(3).HasColumnName("order_num");
            builder.Property(p => p.LineNumber).HasColumnName("line_number");
            builder.Property(p => p.CodeTs).HasMaxLength(2).HasColumnName("code_ts");
            builder.Property(p => p.Created).HasColumnName("created");
            builder.Property(p => p.LastUpdated).HasColumnName("last_updated");
            builder.Property(p => p.IsDeleted).HasDefaultValue(false).HasColumnName("is_deleted");
        }
    }
}