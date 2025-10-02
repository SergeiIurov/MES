using ControlBoard.DB;
using ControlBoard.DB.Entities;
using ControlBoard.Domain.Services.Abstract.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ControlBoard.Domain.Services.Concrete.Settings
{
    public class ScanningPointService(
        ILogger<ScanningPointService> logger,
        MesDbContext context) : IScanningPointService
    {
        public async Task<IEnumerable<ScanningPoint>> GetScanningPointsAsync()
        {
            try
            {
                logger.LogInformation($"Запуск метода {nameof(ScanningPointService)}.");
                IEnumerable<ScanningPoint> result = await context.ScanningPoints.ToListAsync();
                logger.LogInformation($"Метод {nameof(ScanningPointService)} успешно завершен.");
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                return null;
            }
        }
    }
}
