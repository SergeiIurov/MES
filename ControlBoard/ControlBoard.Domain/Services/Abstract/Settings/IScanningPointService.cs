using ControlBoard.DB.Entities;
using ControlBoard.Domain.Dto;

namespace ControlBoard.Domain.Services.Abstract.Settings
{
    public interface IScanningPointService
    {
        Task<IEnumerable<ScanningPoint>> GetScanningPointsAsync();
        Task<ScanningPoint> AddScanningPoiintAsync(ScanningPointDto scanningPoint);
        Task DeleteScanningPointAsync(int id);
        Task<ScanningPoint> UpdateScanningPointAsync(ScanningPointDto scanningPoint);
    }
}
