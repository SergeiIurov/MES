using ControlBoard.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlBoard.DB.Repositories.Concrete
{
    public class AreaRepository : Repository<Area>
    {
        public AreaRepository(MesDbContext db) : base(db)
        {
        }
    }
}
