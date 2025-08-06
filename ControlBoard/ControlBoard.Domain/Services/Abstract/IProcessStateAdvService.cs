using ControlBoard.DB.Entities;
using ControlBoard.Domain.Dto;

namespace ControlBoard.Domain.Services.Abstract;

public interface IProcessStateAdvService
{
    Task SaveListAsync(List<ProcessStateAdvDto> list);
    Task SaveSpecificationAsync(List<(string, string)> data);
    Task AddProcessStateAsync(ProcessStateAdvDto processState);
    Task<List<ProcessState>> GetProcessStatesAsync();
}