using ControlBoard.DB;
using ControlBoard.Domain.Services.Abstract;
using Microsoft.Extensions.Logging;
using ControlBoard.DB.Entities;

namespace ControlBoard.Domain.Services.Concrete
{
    public class HistoryService(ILogger<StationService> logger, MesDbContext context) : IHistoryService
    {
        public async Task WriteHistoryElementAsync(string jsonInfo)
        {
            try
            {
                logger.LogInformation($"Запуск метода {nameof(WriteHistoryElementAsync)}.");
                await context.HistoryInfo.AddAsync(new HistoryInfo() { Created = DateTime.UtcNow, JsonInfo = jsonInfo });
                await context.SaveChangesAsync();
                logger.LogInformation($"Метод {nameof(WriteHistoryElementAsync)} успешно завершен.");
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
            }
        }
    }
}
