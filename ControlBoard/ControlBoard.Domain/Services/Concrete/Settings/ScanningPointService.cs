using ControlBoard.DB;
using ControlBoard.DB.Entities;
using ControlBoard.Domain.Dto;
using ControlBoard.Domain.Services.Abstract.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ControlBoard.Domain.Services.Concrete.Settings
{
    /// <summary>
    /// Сервис для  работы с точками сканирования
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="context"></param>
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

        public async Task<ScanningPoint> AddScanningPoiintAsync(ScanningPointDto scanningPoint)
        {
            ScanningPoint newScanningPoint = new ScanningPoint()
            {
                Name = scanningPoint.Name,
                CodeTs = scanningPoint.CodeTs,
                LineNumber = scanningPoint.LineNumber,
                OrderNum = scanningPoint.OrderNum,
                Created = DateTime.UtcNow,
                LastUpdated = DateTime.UtcNow,
                IsDeleted = false
            };
            await context.ScanningPoints.AddAsync(newScanningPoint);
            await context.SaveChangesAsync();
            return newScanningPoint;
        }

        public async Task DeleteScanningPointAsync(int id)
        {
            context.ScanningPoints.Remove(new ScanningPoint() { Id = id });
            await context.SaveChangesAsync();
        }

        public async Task<ScanningPoint> UpdateScanningPointAsync(ScanningPointDto scanningPoint)
        {
            ScanningPoint sp = await context.ScanningPoints.FindAsync(scanningPoint.Id);
            if (sp is not null)
            {
                sp.Name = scanningPoint.Name;
                sp.CodeTs = scanningPoint.CodeTs;
                sp.LineNumber = scanningPoint.LineNumber;
                sp.OrderNum = scanningPoint.OrderNum;
                sp.LastUpdated = DateTime.UtcNow;
                await context.SaveChangesAsync();
            }

            return sp;
        }
    }
}
