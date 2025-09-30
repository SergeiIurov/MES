using ControlBoard.DB;
using ControlBoard.Domain.Services.Abstract.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ControlBoard.Domain.Services.Concrete.Settings
{
    public class SettingsService(ILogger<SettingsService> logger, MesDbContext context) : ISettingsService
    {
        public async Task<int> GetLineCountAsync()
        {
            try
            {
                logger.LogInformation($"Запуск метода {nameof(SettingsService)}.");
                int lineCount = ((await context.CommonSettings.FirstOrDefaultAsync())!).LineCount;
                logger.LogInformation($"Метод {nameof(SettingsService)} успешно завершен.");
                return lineCount;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
                return 0;
            }
        }
    }
}
