using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static string GetProductType(string? spec, int type)
        {
            return type switch
            {
                1 => GetCabinType(spec),
                2 => GetCarExecution(spec),
                _ => ""
            };
        }
    }
}
