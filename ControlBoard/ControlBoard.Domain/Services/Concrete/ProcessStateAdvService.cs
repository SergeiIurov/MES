using ControlBoard.DB;
using ControlBoard.DB.Entities;
using ControlBoard.DB.Repositories.Abstract;
using ControlBoard.Domain.Dto;
using ControlBoard.Domain.Services.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ControlBoard.Domain.Services.Concrete
{
    public class ProcessStateAdvService(
        IProcessStateRepository repository,
        MesDbContext context,
        ILogger<ProcessStateService> logger) : IProcessStateAdvService
    {
        public async Task SaveListAsync(List<ProcessStateAdvDto> list)
        {
            try
            {
                logger.LogInformation("Подготовка к удалению текущего состояния доски контроля.");
                //Удаляем состояние
                context.Database.ExecuteSql($"truncate table process_states;");

                logger.LogInformation("Подготовка информации к сохранению в БД");
                Guid uid = Guid.NewGuid();
                await repository.SaveProcessStatesAsync(list.Select(s =>
                {
                    return new ProcessState()
                    {
                        Value = s.Value ?? "",
                        Description = "",
                        Created = DateTime.UtcNow,
                        LastUpdated = DateTime.UtcNow,
                        IsDeleted = false,
                        StationId = s.StationId,
                        ProductTypeId = null,
                        GroupId = uid
                    };
                }).ToList());

                logger.LogInformation("Информация сохранена в БД");
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
            }
        }
    }
}