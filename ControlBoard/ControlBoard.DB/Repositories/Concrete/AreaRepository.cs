using ControlBoard.DB.Entities;

namespace ControlBoard.DB.Repositories.Concrete
{
    public class AreaRepository : Repository<Area>
    {
        public AreaRepository(MesDbContext db) : base(db)
        {
        }
    }
}
