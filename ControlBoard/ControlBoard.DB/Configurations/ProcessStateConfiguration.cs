using ControlBoard.DB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlBoard.DB.Configurations
{
    public class ProcessStateConfiguration : IEntityTypeConfiguration<ProcessState>
    {
        public void Configure(EntityTypeBuilder<ProcessState> builder)
        {
            builder.ToTable("ProcessStates").HasQueryFilter(f => !f.IsDeleted);
            builder.Property(p => p.IsDeleted).HasDefaultValue(false).IsRequired();
            builder.Property(p => p.Value).HasMaxLength(150).IsRequired();
            builder.Property(p => p.Created).IsRequired();
            builder.Property(p => p.LastUpdated).IsRequired();
        }
    }
}
