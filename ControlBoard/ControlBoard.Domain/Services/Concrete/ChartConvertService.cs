using ControlBoard.DB.Repositories.Abstract;
using ControlBoard.Domain.Services.Abstract;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;
using ControlBoard.DB;
using ControlBoard.DB.Entities;
using ControlBoard.DB.Entities.Enums;

namespace ControlBoard.Domain.Services.Concrete;

/// <summary>
/// Класс занимается преобразованием сырых XML данных доски с целью
/// наполнения информацией всех элементов интерфейса, имеющих атрибут "sid".
/// Значения данного атрибута сопоставляется с "chart_element_id станции.
/// Также для отображения типа продукта, необходимо добавить атрибут "type" со значениями
/// 1 - тип кабины, 2 - исполнение. 
/// </summary>
public class ChartConvertService(
    IProcessStateRepository repository,
    IProcessStateAdvService processStateAdvService,
    MesDbContext context,
    IChartServices chartService,
    ILogger<ChartConvertService> logger) : IChartConvertService
{
    /// <summary>
    /// Выполняет наполнение схемы доски контроля производства реальными данными для отображения.
    /// </summary>
    /// <param name="from">Сырые данные со схемой доски контроля производства</param>
    public async Task<string> Convert(string from)
    {
        List<Specification> specificationList = await processStateAdvService.GetSpecifications();
        logger.LogInformation($"Запуск метода {nameof(Convert)}.");
        Dictionary<int, (string, ProductTypes?)> dict;
        List<ProcessState> processStates = await repository.GetLastProcessStateAsync();

        if (processStates.Any())
        {
            dict =
                (await repository.GetLastProcessStateAsync()).ToDictionary(s => s.Station!.ChartElementId,
                    d => (d.Value == "null" ? "" : d.Value, d.Station.ProductType));

            XElement root = XElement.Parse(from);
            var data = root.Descendants("object").Where(e => e.Attribute("sid") != null);

            foreach (XElement elem in data)
            {
                try
                {
                    if (dict.TryGetValue(int.TryParse(elem.Attribute("sid")?.Value, out int s) ? s : 0,
                            out (string, ProductTypes?) result))
                    {
                        Specification spec = specificationList.Find(val => val.SequenceNumber == result.Item1);
                        if (specificationList.Exists(val => val.SequenceNumber == result.Item1))
                        {
                            if (result.Item2 != null && result.Item2 != ProductTypes.NotData)
                            {
                                string? info =await chartService.GetProductTypeAsync(spec.SpecificationStr, result.Item2.Value);

                                elem.Attribute("label")!.Value = $"{result.Item1 ?? ""}\n{info}";
                            }
                            else
                            {
                                elem.Attribute("label")!.Value = $"{result.Item1 ?? ""}";
                            }
                        }
                        else
                        {
                            elem.Attribute("label")!.Value = $"{result.Item1 ?? ""}";
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message, ex);
                }
            }

            logger.LogInformation($"Выполнение метода {nameof(Convert)} завершено.");
            return root.ToString();
        }

        return from;
    }

    ///// <summary>
    ///// Получение исполнения
    ///// </summary>
    ///// <param name="spec">На вход подаётся спецификация</param>
    //private static string GetCarExecution(string spec)
    //{
    //    return spec.Substring(4, 2) switch
    //    {
    //        "00" => "ШАССИ",
    //        "10" => "ТЯГАЧ",
    //        "30" => "БОРТ",
    //        "50" => "САМОСВАЛ",
    //        _ => ""
    //    };
    //}

    ///// <summary>
    ///// Получение типа кабины
    ///// </summary>
    ///// <param name="spec">На вход подаётся спецификация </param>
    //private static string GetCabinType(string spec)
    //{
    //    return spec.Substring(2, 1) switch
    //    {
    //        "1" => "ККН",
    //        "2" => "ККС",
    //        "3" => "ДКН",
    //        "4" => "ДКС",
    //        "5" => "ДКВ",
    //        _ => ""
    //    };
    //}
}