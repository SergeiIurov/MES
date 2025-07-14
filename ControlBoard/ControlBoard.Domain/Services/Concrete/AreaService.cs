using ControlBoard.DB.Entities;
using ControlBoard.Domain.Services.Abstract;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlBoard.DB.Repositories.Concrete;

namespace ControlBoard.Domain.Services.Concrete
{
    public class AreaService(AreaRepository repository, ILogger<AreaService> logger) : IAreaService
    {
       public async Task<List<Area>> GetAreasAsync()
        {
            logger.LogInformation("Получение списка станций.");
            return await repository.GetAllAsync();
        }
    }
}
