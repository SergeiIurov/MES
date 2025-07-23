using ControlBoard.DB.Entities;
using ControlBoard.Domain.Services.Abstract;
using Microsoft.Extensions.Logging;
using ControlBoard.DB.Repositories.Concrete;

namespace ControlBoard.Domain.Services.Concrete
{
    public class ProductTypeService(ProductTypeRepository repository, ILogger<ProductTypeService> logger) : IProductTypeService
    {
        public async Task<IEnumerable<ProductType>> GetProductTypesAsync()
        {
            logger.LogInformation("Получение списка станций.");
            return await repository.GetAllAsync();
        }

    }
}
