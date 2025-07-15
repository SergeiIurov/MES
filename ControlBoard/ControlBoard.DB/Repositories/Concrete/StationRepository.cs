using ControlBoard.DB.Entities;

namespace ControlBoard.DB.Repositories.Concrete
{
    public class StationRepository : Repository<Station>
    {
        public StationRepository(MesDbContext db) : base(db)
        {
        }
    }
}
