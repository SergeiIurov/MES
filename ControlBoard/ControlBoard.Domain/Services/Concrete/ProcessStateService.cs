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

namespace ControlBoard.Domain.Services.Concrete
{
    public class ProcessStateService(IProcessStateRepository repository, MesDbContext context) : IProcessStateService
    {
        private Dictionary<string, int> _stationMapper = context.Stations.ToDictionary(s => s.Name, s => s.Id, new CaseInsensitiveValueComparer());
        private Dictionary<string, int> _productTypeMapper = context.ProductTypes.ToDictionary(p => p.Name, p => p.Id, new CaseInsensitiveValueComparer());
        public async Task SaveListAsync(List<ProcessStateDto> list)
        {
            Guid uid = Guid.NewGuid();
            await repository.SaveProcessStates(list.Select(s =>
              {
                  return new ProcessState()
                  {
                      Value = s.Value,
                      Description = "",
                      Created = DateTime.Now,
                      LastUpdated = DateTime.Now,
                      IsDeleted = false,
                      StationId = _stationMapper[s.StationName],
                      ProductTypeId = _productTypeMapper[s.ProductTypeName],
                      GroupId = uid
                  };
              }).ToList());
        }
    }
}
