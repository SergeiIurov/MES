using ControlBoard.DB.Entities.Enums;
using ControlBoard.Domain.Services.Abstract;

namespace ControlBoard.Domain.Services.Concrete
{
    public class ChartServices(ICarExecutionService carExecutionService) : IChartServices
    {
        /// <summary>
        /// Получение исполнения
        /// </summary>
        /// <param name="spec">На вход подаётся спецификация</param>
        public async Task<string?> GetCarExecutionAsync(string? spec)
        {
            Dictionary<string, string?> dict =
                (await carExecutionService.GetCarExecutionsAsync()).ToDictionary(ce => ce.Code, ce => ce.Name);
            if (spec is null)
            {
                return "";
            }

            return dict.TryGetValue(spec.Substring(4, 2), out string? value) ? value : "";
        }

        /// <summary>
        /// Получение типа кабины
        /// </summary>
        /// <param name="spec">На вход подаётся спецификация </param>
        public string? GetCabinType(string? spec)
        {
            if (spec is null)
            {
                return "";
            }
            return spec.Substring(2, 1) switch
            {
                "1" => "ККН",
                "2" => "ККС",
                "3" => "ДКН",
                "4" => "ДКС",
                "5" => "ДКВ",
                _ => ""
            };
        }

        public async Task<string?> GetProductTypeAsync(string? spec, ProductTypes productType)
        {
            return productType switch
            {
                ProductTypes.Cabina => GetCabinType(spec),
                ProductTypes.Execution => await GetCarExecutionAsync(spec),
                _ => ""
            };
        }
    }
}
