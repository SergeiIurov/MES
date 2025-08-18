using ControlBoard.DB.Entities;
using ControlBoard.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlBoard.Domain.Services.Abstract
{
    public interface ICarExecutionService
    {
        Task<List<CarExecution>> GetCarExecutionsAsync();
        Task<CarExecution> AddCarExecutionAsync(CarExecutionDto carExecution);
        Task DeleteExecutionAxync(int id);
        Task<CarExecution> UpdateCarExecutionAsync(CarExecutionDto carExecution);
    }
}
