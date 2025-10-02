using ControlBoard.DB.Entities;

namespace ControlBoard.Domain.Services.Abstract.Settings
{
    public interface IScanningPointService
    {
        Task<IEnumerable<ScanningPoint>> GetScanningPointsAsync();
    }
}
