using ControlBoard.DB.Repositories.Abstract;
using ControlBoard.Domain.Dto;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlBoard.DB;
using ControlBoard.DB.Entities;
using ControlBoard.Domain.Services.Abstract;
using Microsoft.Extensions.Logging;

namespace ControlBoard.Domain.Services.Concrete
{
    public class ProcessStateService(IProcessStateRepository repository, MesDbContext context, ILogger<ProcessStateService> logger) : IProcessStateService
    {
        private Dictionary<string, int> _stationMapper = context.Stations.ToDictionary(s => s.Name, s => s.Id, new CaseInsensitiveValueComparer());
        private Dictionary<string, int> _productTypeMapper = context.ProductTypes.ToDictionary(p => p.Name, p => p.Id, new CaseInsensitiveValueComparer());
        public async Task SaveListAsync(List<ProcessStateDto> list)
        {
            try
            {
                logger.LogInformation("Подготовка информации к сохранению в БД");
                Guid uid = Guid.NewGuid();
                await repository.SaveProcessStates(list.Select(s =>
                {
                    _productTypeMapper.TryGetValue(s.ProductTypeName, out int id);
                    return new ProcessState()
                    {
                        Value = s.Value ?? "",
                        Description = "",
                        Created = DateTime.Now.ToUniversalTime(),
                        LastUpdated = DateTime.Now.ToUniversalTime(),
                        IsDeleted = false,
                        StationId = _stationMapper[s.StationName],
                        ProductTypeId = id != 0 ? id : null,
                        GroupId = uid
                    };
                }).ToList());

                logger.LogInformation("Информация сохранена в БД");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
            }
        }
    }
}
