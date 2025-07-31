using System.Text.RegularExpressions;
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
        public async Task<bool> IsFreeAsync(int id, int chartElementId)
        {
            Station? s = await context.Stations.FindAsync(id);
            if (s != null)
            {
                if (chartElementId == s.ChartElementId)
                {
                    //Не проверяем на уникальность, данные уже в БД, и вероятно, что корректные.
                    return true;
                }

                return !await context.Stations.AnyAsync(s => s.ChartElementId == chartElementId);
            }

            return !await context.Stations.AnyAsync(s => s.ChartElementId == chartElementId);
        }

        //Проверка на корректность ID для заданного диапазона
        public async Task<bool> IsInRangeAsync(int id, int areaId, int chartElementId)
        {
            Station? s = await context.Stations.FindAsync(id);
            if (s != null)
            {
                if (chartElementId == s.ChartElementId)
                {
                    //Не проверяем на уникальность, данные уже в БД, и вероятно, что корректные.
                    return true;
                }

                string? range = s.Area.Range;
                var res = Regex.Match(range, @"^(\d+).+?(\d+)$");
                int from = int.Parse(res.Groups[1].Value);
                int to = int.Parse(res.Groups[2].Value);
                return chartElementId >= from && chartElementId <= to;

            }
            else
            {
                Area? area = await context.Areas.Where(a => a.Id == areaId).FirstOrDefaultAsync();
                string? range = area.Range;
                var res = Regex.Match(range, @"^(\d+).+?(\d+)$");
                int from = int.Parse(res.Groups[1].Value);
                int to = int.Parse(res.Groups[2].Value);
                return chartElementId >= from && chartElementId <= to;
            }

        }
    }
}