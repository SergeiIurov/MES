using ControlBoard.Domain.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlBoard.DB.Entities;
using ControlBoard.DB.Repositories.Concrete;
using Microsoft.Extensions.Logging;

namespace ControlBoard.Domain.Services.Concrete
{
    public class StationService(StationRepository repository, ILogger<StationService> logger) : IStationService
    {
        public async Task<IEnumerable<Station>> GetStationsAsync()
        {
            logger.LogInformation("Получение списка станций.");
            return await repository.GetAllAsync();
        }
    }
}
