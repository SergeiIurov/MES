using ControlBoard.DB.Configurations;
using ControlBoard.DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace ControlBoard.DB
{
    public class MesDbContext : DbContext
    {
        public MesDbContext(DbContextOptions<MesDbContext> options) : base(options) { }

        public DbSet<Area> Areas { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<ProcessState> ProcessStates { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<ControlBoardData> ControlBoardData { get; set; }
        public DbSet<HistoryInfo> HistoryInfo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AreaConfiguration());
            modelBuilder.ApplyConfiguration(new StationConfiguration());
            modelBuilder.ApplyConfiguration(new ProcessStateConfiguration());
            modelBuilder.ApplyConfiguration(new ProductTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ControlBoardDataConfiguration());
            modelBuilder.ApplyConfiguration(new HistoryInfoConfiguration());
            modelBuilder.ApplyConfiguration(new SpecificationConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
