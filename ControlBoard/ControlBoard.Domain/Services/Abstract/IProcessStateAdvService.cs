using ControlBoard.DB.Entities;
using ControlBoard.Domain.Dto;

namespace ControlBoard.Domain.Services.Abstract;

public interface IProcessStateAdvService
{
    Task SaveListAsync(List<ProcessStateAdvDto> list, string userName);
    Task SaveSpecificationAsync(List<(string, string)> data);
    Task<List<Specification>> GetSpecifications();
    Task AddProcessStateAsync(ProcessStateAdvDto processState);
    Task<List<ProcessState>> GetProcessStatesAsync();
    Task<bool> ChangeDisabledStatus(AreaDto areaDto);
}