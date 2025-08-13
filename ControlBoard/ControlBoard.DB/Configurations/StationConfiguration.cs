using ControlBoard.DB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlBoard.DB.Configurations
{
    public class StationConfiguration : IEntityTypeConfiguration<Station>
    {
        public void Configure(EntityTypeBuilder<Station> builder)
        {
            builder.ToTable("stations").HasQueryFilter(f => !f.IsDeleted).HasIndex(e => e.ChartElementId).IsUnique();
            builder.Property(p => p.Id).HasColumnName("id");
            builder.Property(p => p.ChartElementId).HasColumnName("chart_element_id");
            builder.Property(p => p.ProductType).HasColumnName("product_type");
            builder.Property(p => p.Name).HasMaxLength(150).HasColumnName("name");
            builder.Property(p => p.Description).HasColumnName("description");
            builder.Property(p => p.Created).HasColumnName("created");
            builder.Property(p => p.LastUpdated).HasColumnName("last_updated");
            builder.Property(p => p.IsDeleted).HasDefaultValue(false).HasColumnName("is_deleted");
            builder.Property(p => p.AreaId).HasColumnName("area_id");
        }
    }
}
