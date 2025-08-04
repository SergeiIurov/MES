using System.Text.Json;
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
        ILogger<ProcessStateService> logger,
        IHistoryService historyService) : IProcessStateAdvService
    {
        public async Task SaveListAsync(List<ProcessStateAdvDto> list)
        {
            try
            {
                logger.LogInformation("Подготовка к удалению текущего состояния доски контроля.");
                //Удаляем состояние
                context.Database.ExecuteSql($"truncate table process_states;");

                logger.LogInformation("Подготовка информации к сохранению в БД.");
                Guid uid = Guid.NewGuid();
                await repository.SaveProcessStatesAsync(list.Select(s =>
                {
                    return new ProcessState()
                    {
                        Value = string.IsNullOrEmpty(s.Value) || s.Value.Equals("null") ? "" : s.Value,
                        Description = "",
                        Created = DateTime.UtcNow,
                        LastUpdated = DateTime.UtcNow,
                        IsDeleted = false,
                        StationId = s.StationId,
                        ProductTypeId = null,
                        GroupId = uid
                    };
                }).ToList());

                logger.LogInformation("Информация сохранена в БД.");

                logger.LogInformation("Подготовка записи истории.");
                await historyService.WriteHistoryElementAsync(JsonSerializer.Serialize(context.ProcessStates.OrderBy(ps => ps.Id).Select(ps => new
                {
                    Value = string.IsNullOrEmpty(ps.Value) || ps.Value.Equals("null") ? null : ps.Value,
                    ps.Created,
                    ps.LastUpdated,
                    Area = ps.Station.Area.Name,
                    Station = ps.Station.Name,
                    ProductType = ps.ProductType.Name,
                    ps.GroupId
                })));

                logger.LogInformation("Запись истории выполнена.");
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
            }
        }

        public async Task AddProcessStateAsync(ProcessStateAdvDto processState)
        {
            await context.ProcessStates.AddAsync(new ProcessState
            {
                Value = string.IsNullOrEmpty(processState.Value) || processState.Value.Equals("null") ? "" : processState.Value,
                Description = "",
                Created = DateTime.UtcNow,
                LastUpdated = DateTime.UtcNow,
                IsDeleted = false,
                StationId = processState.StationId,
                ProductTypeId = null,
                GroupId = Guid.Empty
            });
        }

        public async Task<List<ProcessState>> GetProcessStatesAsync()
        {
            try
            {
                logger.LogInformation($"Запуск метода {nameof(GetProcessStatesAsync)}.");
                return await context.ProcessStates.ToListAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
                throw;
            }
        }
    }
}