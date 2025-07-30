using ControlBoard.DB;
using ControlBoard.DB.Entities;
using ControlBoard.DB.Repositories.Concrete;
using ControlBoard.Domain.Dto;
using ControlBoard.Domain.Services.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ControlBoard.Domain.Services.Concrete
{
    public class StationService(StationRepository repository, ILogger<StationService> logger, MesDbContext context)
        : IStationService
    {
        public async Task<IEnumerable<Station>> GetStationsAsync()
        {
            logger.LogInformation("Получение списка станций.");
            return await repository.GetAllAsync();
        }

        public async Task<Station> AddStationAsync(StationDto station)
        {
            Station newStation = new Station()
            {
                Name = station.Name,
                ChartElementId = station.ChartElementId,
                AreaId = station.AreaId,
                Created = DateTime.UtcNow,
                LastUpdated = DateTime.UtcNow,
                Description = "",
                IsDeleted = false,
            };
            await context.Stations.AddAsync(newStation);

            await context.SaveChangesAsync();
            return newStation;
        }

        public async Task DeleteStationAxync(int id)
        {
            context.Stations.Remove(new Station() { Id = id });
            await context.SaveChangesAsync();
        }

        public async Task<Station> UpdateStationAsync(StationDto station)
        {
            Station s = await context.Stations.FindAsync(station.Id);
            if (s != null)
            {
                s.Name = station.Name;
                s.ChartElementId = station.ChartElementId;
                s.LastUpdated = DateTime.UtcNow;
                await context.SaveChangesAsync();
            }

            return s;
        }

        /// <summary>
        /// Проверяем, является ли свободным идентификатор станции (ChartElementId)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> IsFree(int id)
        {
            return !await context.Stations.Where(s => s.ChartElementId == id).AnyAsync();
        }
    }
}