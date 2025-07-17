using ControlBoard.DB;
using ControlBoard.DB.Entities;
using ControlBoard.DB.Repositories.Abstract;
using ControlBoard.Domain.Dto;
using ControlBoard.Domain.Services.Abstract;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace ControlBoard.Domain.Services.Concrete
{
    public class ProcessStateService(
        IProcessStateRepository repository,
        MesDbContext context,
        ILogger<ProcessStateService> logger) : IProcessStateService
    {
        private Dictionary<string, int> _stationMapper =
            context.Stations.ToDictionary(s => s.Name, s => s.Id, new CaseInsensitiveValueComparer());

        private Dictionary<string, int> _productTypeMapper =
            context.ProductTypes.ToDictionary(p => p.Name, p => p.Id, new CaseInsensitiveValueComparer());

        public async Task SaveListAsync(List<ProcessStateDto> list)
        {
            try
            {
                logger.LogInformation("Подготовка информации к сохранению в БД");
                Guid uid = Guid.NewGuid();
                await repository.SaveProcessStatesAsync(list.Select(s =>
                {
                    _productTypeMapper.TryGetValue(s.ProductTypeName, out int id);
                    return new ProcessState()
                    {
                        Value = s.Value ?? "",
                        Description = "",
                        Created = DateTime.UtcNow,
                        LastUpdated = DateTime.UtcNow,
                        IsDeleted = false,
                        StationId = s.StationName != null && _stationMapper.TryGetValue(s.StationName, out int res) ? res : null,
                        ProductTypeId = id != 0 ? id : null,
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