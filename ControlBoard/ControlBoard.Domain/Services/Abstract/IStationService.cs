using ControlBoard.DB.Entities;

namespace ControlBoard.Domain.Services.Abstract
{
    public interface IStationService
    {
        Task<IEnumerable<Station>> GetStationsAsync();
    }
}