using ControlBoard.DB;
using ControlBoard.DB.Entities;
using ControlBoard.DB.Repositories.Abstract;
using ControlBoard.Domain.Dto;
using ControlBoard.Domain.Services.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using ControlBoard.DB.Entities.Enums;

namespace ControlBoard.Domain.Services.Concrete
{
    public class ProcessStateAdvService(
        IProcessStateRepository repository,
        MesDbContext context,
        ILogger<ProcessStateService> logger,
        IChartServices chartServices,
        IHistoryService historyService) : IProcessStateAdvService
    {
        public async Task SaveListAsync(List<ProcessStateAdvDto> list, string userName)
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

                List<Specification> specificationList = await GetSpecifications();

                await historyService.WriteHistoryElementAsync(JsonSerializer.Serialize(context.ProcessStates.Include(s => s.Station).ToList().OrderBy(ps => ps.Id).Select(async ps =>
                 new
                 {
                     Value = string.IsNullOrEmpty(ps.Value) || ps.Value.Equals("null") ? null : ps.Value,
                     ps.Created,
                     ps.LastUpdated,
                     Area = ps.Station.Area.Name,
                     Station = ps.Station.Name,
                     ProductType = await 
                         chartServices.GetProductTypeAsync(
                             specificationList.FirstOrDefault(s =>
                                 s.SequenceNumber.Equals(ps.Value))?.SpecificationStr, ps.Station.ProductType ?? ProductTypes.NotData),
                     ps.GroupId,
                     Login = userName
                 })));

                logger.LogInformation("Запись истории выполнена.");
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
            }
        }

        public async Task SaveSpecificationAsync(List<(string, string)> data)
        {
            try
            {
                logger.LogInformation("Подготовка к удалению текущей спецификации.");
                //Удаляем спецификацию
                context.Database.ExecuteSql($"truncate table specification;");

                logger.LogInformation("Подготовка спецификации к сохранению в БД.");


                await context.Specification.AddRangeAsync(data.Select(s =>
                {
                    return new Specification()
                    {
                        SequenceNumber = s.Item1,
                        SpecificationStr = s.Item2,
                        Created = DateTime.UtcNow,
                        LastUpdated = DateTime.UtcNow,
                        IsDeleted = false
                    };
                }));

                await context.SaveChangesAsync();

                logger.LogInformation("Информация по спецификации сохранена в БД.");


            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
            }
        }

        public async Task<List<Specification>> GetSpecifications()
        {
            return await context.Specification.ToListAsync();
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