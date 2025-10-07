using ControlBoard.DB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlBoard.DB.Configurations
{
    public class ProductionProcessConfiguration : IEntityTypeConfiguration<ProductionProcess>
    {
        public void Configure(EntityTypeBuilder<ProductionProcess> builder)
        {
            builder.ToTable("areas").HasQueryFilter(f => !f.IsDeleted);
            builder.Property(p => p.Id).HasColumnName("id");
            builder.Property(p => p.LineNumber).HasColumnName("line_number");
            builder.Property(p => p.ProductType).HasMaxLength(255).HasColumnName("product_type");
            builder.Property(p => p.Comment).HasColumnName("comment");
            builder.Property(p => p.Created).HasColumnName("created");
            builder.Property(p => p.LastUpdated).HasColumnName("last_updated");
            builder.Property(p => p.IsDeleted).HasDefaultValue(false).HasColumnName("is_deleted");
        }
    }
}