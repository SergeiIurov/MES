using ControlBoard.DB.Entities;

namespace ControlBoard.Domain.Services.Abstract
{
    public interface IAreaService
    {
        Task<List<Area>> GetAreasAsync();
    }
}
