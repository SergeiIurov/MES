using ControlBoard.DB;
using ControlBoard.DB.Entities;
using ControlBoard.Domain.Dto;
using ControlBoard.Domain.Services.Abstract;
using Microsoft.Extensions.Logging;

using Microsoft.EntityFrameworkCore;

namespace ControlBoard.Domain.Services.Concrete
{
    public class CarExecutionService(ILogger<CarExecutionService> logger, MesDbContext context) : ICarExecutionService
    {
        public async Task<List<CarExecution>> GetCarExecutionsAsync()
        {
            logger.LogInformation("Получение списка надстроек.");
            return await context.CarExecution.OrderBy(a => a.Code).ToListAsync();
        }

        public async Task<CarExecution> AddCarExecutionAsync(CarExecutionDto carExecution)
        {
            CarExecution newCarExecution = new CarExecution()
            {
                Name = carExecution.Name,
                Code = carExecution.Code,
                Created = DateTime.UtcNow,
                LastUpdated = DateTime.UtcNow,
                Description = "",
                IsDeleted = false,
            };
            await context.CarExecution.AddAsync(newCarExecution);

            await context.SaveChangesAsync();
            return newCarExecution;
        }

        public async Task DeleteExecutionAxync(int id)
        {
            context.CarExecution.Remove(new CarExecution() { Id = id });
            await context.SaveChangesAsync();
        }

        public async Task<CarExecution> UpdateCarExecutionAsync(CarExecutionDto carExecution)
        {
            CarExecution c = await context.CarExecution.FindAsync(carExecution.Id);
            if (c != null)
            {
                c.Name = carExecution.Name;
                c.Code = carExecution.Code;
                c.LastUpdated = DateTime.UtcNow;
                await context.SaveChangesAsync();
            }

            return c;
        }
    }
}
