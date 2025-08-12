using ControlBoard.DB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlBoard.DB.Configurations
{
    public class HistoryInfoConfiguration : IEntityTypeConfiguration<HistoryInfo>
    {
        public void Configure(EntityTypeBuilder<HistoryInfo> builder)
        {
            builder.ToTable("history_info").HasQueryFilter(f => !f.IsDeleted);
            builder.Property(p => p.Id).HasColumnName("id");
            builder.Property(p => p.JsonInfo).HasColumnName("json_info");
            builder.Property(p => p.IsDeleted).HasDefaultValue(false).HasColumnName("is_deleted");
        }
    }
}
