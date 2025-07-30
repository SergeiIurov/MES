using ControlBoard.DB;
using ControlBoard.DB.Entities;
using ControlBoard.Domain.Services.Abstract;
using Microsoft.Extensions.Logging;
using ControlBoard.DB.Repositories.Concrete;
using ControlBoard.Domain.Dto;
using Microsoft.EntityFrameworkCore;

namespace ControlBoard.Domain.Services.Concrete
{
    public class AreaService(AreaRepository repository, ILogger<AreaService> logger, MesDbContext context) : IAreaService
    {
        public async Task<List<Area>> GetAreasAsync()
        {
            logger.LogInformation("Получение списка станций.");
            return await context.Areas.OrderBy(a => a.Id).ToListAsync();
        }

        public async Task<Area> AddAreaAsync(AreaDto area)
        {
            Area newArea = new Area()
            {
                Name = area.Name,
                Created = DateTime.UtcNow,
                LastUpdated = DateTime.UtcNow,
                Description = "",
                IsDeleted = false,
            };
            await context.Areas.AddAsync(newArea);

            await context.SaveChangesAsync();
            return newArea;
        }

        public async Task DeleteAreaAxync(int id)
        {
            context.Areas.Remove(new Area() { Id = id });
            await context.SaveChangesAsync();
        }

        public async Task<Area> UpdateAreaAsync(AreaDto area)
        {
            Area a = await context.Areas.FindAsync(area.Id);
            if (a != null)
            {
                a.Name = area.Name;
                a.LastUpdated = DateTime.UtcNow;
                await context.SaveChangesAsync();
            }

            return a;
        }
    }
}
