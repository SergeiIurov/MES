using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
