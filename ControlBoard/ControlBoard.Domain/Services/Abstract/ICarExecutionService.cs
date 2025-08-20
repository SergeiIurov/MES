using ControlBoard.DB.Entities;
using ControlBoard.Domain.Dto;

namespace ControlBoard.Domain.Services.Abstract
{
    public interface ICarExecutionService
    {
        Task<List<CarExecution>> GetCarExecutionsAsync();
        Task<CarExecution> AddCarExecutionAsync(CarExecutionDto carExecution);
        Task DeleteExecutionAsync(int id);
        Task<CarExecution> UpdateCarExecutionAsync(CarExecutionDto carExecution);
    }
}
