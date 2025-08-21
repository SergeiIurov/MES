using ControlBoard.DB.Entities.Enums;

namespace ControlBoard.Domain.Services.Abstract
{
    public interface IChartServices
    {
        /// <summary>
        /// Получение исполнения
        /// </summary>
        /// <param name="spec">На вход подаётся спецификация</param>
        Task<string?> GetCarExecutionAsync(string? spec);
        Task<string?> GetProductTypeAsync(string? spec, ProductTypes productType);
        public string? GetCabinType(string? spec);

    }
}
