using ControlBoard.DB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlBoard.DB.Configurations
{
    public class ProcessStateConfiguration : IEntityTypeConfiguration<ProcessState>
    {
        public void Configure(EntityTypeBuilder<ProcessState> builder)
        {
            builder.ToTable("process_states").HasQueryFilter(f => !f.IsDeleted);
            builder.Property(p => p.Id).HasColumnName("id");
            builder.Property(p => p.Value).HasMaxLength(150).HasColumnName("value");
            builder.Property(p => p.Description).HasColumnName("description");
            builder.Property(p => p.Created).HasColumnName("created");
            builder.Property(p => p.LastUpdated).HasColumnName("last_updated");
            builder.Property(p => p.IsDeleted).HasDefaultValue(false).HasColumnName("is_deleted");
            builder.Property(p => p.StationId).HasColumnName("station_id");
            builder.Property(p => p.ProductTypeId).HasColumnName("product_type_id");
            builder.Property(p => p.GroupId).HasColumnName("group_id");
        }
    }
}
