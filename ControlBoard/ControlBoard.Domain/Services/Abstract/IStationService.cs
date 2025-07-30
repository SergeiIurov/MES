using ControlBoard.DB.Entities;
using ControlBoard.Domain.Dto;

namespace ControlBoard.Domain.Services.Abstract
{
    public interface IStationService
    {
        Task<IEnumerable<Station>> GetStationsAsync();
        Task<Station> AddStationAsync(StationDto station);
        Task DeleteStationAxync(int id);
        Task<Station> UpdateStationAsync(StationDto station);
        Task<bool> IsFree(int id);
    }
}