using ControlBoard.DB.Entities;
using ControlBoard.DB.Repositories.Concrete;
using ControlBoard.Domain.Services.Abstract;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlBoard.Domain.Services.Concrete
{
    public class BoardConstructorService(BoardConstructorRepository repository, ILogger<StationService> logger) : IBoardConstructorService
    {
        public async Task<int> UpdateLastDataOrCreateAsync(string chart)
        {
            logger.LogInformation("Подготовка к обновлению состояния конструктора.");
            ControlBoardData data = new() { Created = DateTime.UtcNow, LastUpdated = DateTime.UtcNow, Data = chart };
            int id = await repository.UpdateLastDataOrCreateAsync(data);
            logger.LogInformation("Обновление состояния конструктора выполнено.");
            return id;
        }

        public async Task<string> GetLastDataAsync()
        {
            logger.LogInformation($"Отработка метода {nameof(GetLastDataAsync)}.");
            return (await repository.GetLastData())?.Data ?? "";
        }
    }
}
