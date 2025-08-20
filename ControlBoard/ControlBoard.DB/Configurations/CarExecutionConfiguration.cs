using ControlBoard.DB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlBoard.DB.Configurations
{
    public class CarExecutionConfiguration : IEntityTypeConfiguration<CarExecution>
    {
        public void Configure(EntityTypeBuilder<CarExecution> builder)
        {
            builder.ToTable("car_execution").HasQueryFilter(f => !f.IsDeleted);
            builder.Property(p => p.Id).HasColumnName("id");
            builder.Property(p => p.Code).HasColumnName("code").HasMaxLength(2).IsFixedLength();
            builder.Property(p => p.Name).HasMaxLength(150).HasColumnName("name");
            builder.Property(p => p.Description).HasColumnName("description");
            builder.Property(p => p.Created).HasColumnName("created");
            builder.Property(p => p.LastUpdated).HasColumnName("last_updated");
            builder.Property(p => p.IsDeleted).HasDefaultValue(false).HasColumnName("is_deleted");
        }
    }
}
