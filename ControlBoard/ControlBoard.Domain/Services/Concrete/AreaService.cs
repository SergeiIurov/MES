using ControlBoard.DB;
using ControlBoard.DB.Entities;
using ControlBoard.Domain.Services.Abstract;
using Microsoft.Extensions.Logging;
using ControlBoard.Domain.Dto;
using Microsoft.EntityFrameworkCore;

namespace ControlBoard.Domain.Services.Concrete
{
    public class AreaService(ILogger<AreaService> logger, MesDbContext context) : IAreaService
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
                Range = area.Range,
                Created = DateTime.UtcNow,
                LastUpdated = DateTime.UtcNow,
                Description = "",
                IsDeleted = false,
                DisabledColor = area.DisabledColor
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
                a.Range = area.Range;
                a.LastUpdated = DateTime.UtcNow;
                await context.SaveChangesAsync();
            }

            return a;
        }

        public async Task SetDisabledColorAsync(int id, string color)
        {
            Area a = await context.Areas.FindAsync(id);
            if (a is not null)
            {
                a.DisabledColor = color;
                await context.SaveChangesAsync();
            }
        }
    }
}
