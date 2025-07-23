using ControlBoard.DB.Entities;

namespace ControlBoard.Domain.Services.Abstract
{
    public interface IProductTypeService
    {
        Task<IEnumerable<ProductType>> GetProductTypesAsync();
    }
}
