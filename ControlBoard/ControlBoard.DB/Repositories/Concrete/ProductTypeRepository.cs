using ControlBoard.DB.Entities;

namespace ControlBoard.DB.Repositories.Concrete
{
    public class ProductTypeRepository : Repository<ProductType>
    {
        public ProductTypeRepository(MesDbContext db) : base(db)
        {
        }
    }
}
