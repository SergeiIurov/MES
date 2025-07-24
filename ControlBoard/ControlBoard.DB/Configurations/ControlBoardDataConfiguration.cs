using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ControlBoard.DB.Entities;

namespace ControlBoard.DB.Configurations
{
    public class ControlBoardDataConfiguration : IEntityTypeConfiguration<ControlBoardData>
    {
        public void Configure(EntityTypeBuilder<ControlBoardData> builder)
        {
            builder.ToTable("control_board_data").HasQueryFilter(f => !f.IsDeleted);
            builder.Property(p => p.Id).HasColumnName("id");
            builder.Property(p => p.Data).HasColumnName("data");
            builder.Property(p => p.Created).HasColumnName("created");
            builder.Property(p => p.LastUpdated).HasColumnName("last_updated");
            builder.Property(p => p.IsDeleted).HasDefaultValue(false).HasColumnName("is_deleted");
        }
    }
}
