using ControlBoard.DB.Entities;
using ControlBoard.Domain.Dto;

namespace ControlBoard.Domain.Services.Abstract
{
    public interface IAreaService
    {
        Task<List<Area>> GetAreasAsync();
        Task<Area> AddAreaAsync(AreaDto area);
        Task DeleteAreaAxync(int id);
        Task<Area> UpdateAreaAsync(AreaDto area);
    }
}
