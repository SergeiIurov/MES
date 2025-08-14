using ControlBoard.DB.Entities.Enums;

namespace ControlBoard.Domain.Services
{
    public class ChartServices
    {
        /// <summary>
        /// Получение исполнения
        /// </summary>
        /// <param name="spec">На вход подаётся спецификация</param>
        private static string GetCarExecution(string? spec)
        {
            if (spec is null)
            {
                return "";
            }
            return spec.Substring(4, 2) switch
            {
                "00" => "ШАССИ",
                "10" => "ТЯГАЧ",
                "30" => "БОРТ",
                "50" => "САМОСВАЛ",
                _ => ""
            };
        }

        /// <summary>
        /// Получение типа кабины
        /// </summary>
        /// <param name="spec">На вход подаётся спецификация </param>
        private static string GetCabinType(string? spec)
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

        public static string GetProductType(string? spec, ProductTypes productType)
        {
            return productType switch
            {
                ProductTypes.Cabina => GetCabinType(spec),
                ProductTypes.Execution => GetCarExecution(spec),
                _ => ""
            };
        }
    }
}
