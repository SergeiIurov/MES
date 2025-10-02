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
        public DbSet<Specification> Specification { get; set; }
        public DbSet<CarExecution> CarExecution { get; set; }
        public DbSet<CommonSettings> CommonSettings { get; set; }
        public DbSet<ScanningPoint> ScanningPoints { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AreaConfiguration());
            modelBuilder.ApplyConfiguration(new StationConfiguration());
            modelBuilder.ApplyConfiguration(new ProcessStateConfiguration());
            modelBuilder.ApplyConfiguration(new ProductTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ControlBoardDataConfiguration());
            modelBuilder.ApplyConfiguration(new HistoryInfoConfiguration());
            modelBuilder.ApplyConfiguration(new SpecificationConfiguration());
            modelBuilder.ApplyConfiguration(new CarExecutionConfiguration());
            modelBuilder.ApplyConfiguration(new CommonSettingsConfiguration());
            modelBuilder.ApplyConfiguration(new ScanningPointConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
