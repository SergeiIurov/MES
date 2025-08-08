using ControlBoard.DB.Repositories.Abstract;
using ControlBoard.Domain.Services.Abstract;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;
using ControlBoard.DB.Entities;

namespace ControlBoard.Domain.Services.Concrete;

/// <summary>
/// Класс занимается преобразованием сырых XML данных доски с целью
/// наполнения информацией всех элементов интерфейса, имеющих атрибут "sid".
/// Значения данного атрибута сопоставляется с id станции. 
/// </summary>
/// <param name="repository"></param>
public class ChartConvertService(
    IProcessStateRepository repository,
    IProcessStateAdvService processStateAdvService,
    ILogger<ChartConvertService> logger) : IChartConvertService
{
    public async Task<string> Convert(string from)
    {
        List<Specification> specificationList = await processStateAdvService.GetSpecifications();
        logger.LogInformation($"Запуск метода {nameof(Convert)}.");
        //Dictionary<int, (string, string)> dict;
        Dictionary<int, string> dict;
        List<ProcessState> processStates = await repository.GetLastProcessStateAsync();
        if (processStates.Any())
        {
            //dict =
            //    (await repository.GetLastProcessStateAsync()).ToDictionary(s => s.Station!.ChartElementId,
            //        d => (d.Value == "null" ? "" : d.Value, d.ProductType?.Name ?? ""));
            dict =
                (await repository.GetLastProcessStateAsync()).ToDictionary(s => s.Station!.ChartElementId,
                    d => d.Value == "null" ? "" : d.Value);



            XElement root = XElement.Parse(from);
            var data = root.Descendants("object").Where(e => e.Attribute("sid") != null);

            foreach (XElement elem in data)
            {
                //try
                //{
                //    if (dict.TryGetValue(int.TryParse(elem.Attribute("sid")?.Value, out int s) ? s : 0,
                //            out (string, string) result))
                //    {
                //        if (!string.IsNullOrEmpty(result.Item2))
                //        {
                //            elem.Attribute("label")!.Value = $"{result.Item1 ?? ""}\n{result.Item2}";
                //        }
                //        else
                //        {
                //            elem.Attribute("label")!.Value = $"{result.Item1 ?? ""}";
                //        }
                //    }
                //}
                try
                {
                    if (dict.TryGetValue(int.TryParse(elem.Attribute("sid")?.Value, out int s) ? s : 0,
                            out string result))
                    {
                        Specification spec = specificationList.Find(val => val.SequenceNumber == result);
                        if (specificationList.Exists(val => val.SequenceNumber == result))
                        {
                            var r = elem.Attribute("type")?.Value;
                            int type = int.TryParse(elem.Attribute("type")?.Value, out int typeId) ? typeId : 0;
                            if (type != 0)
                            {
                                string info = type switch
                                {
                                    1 => GetCabinType(spec.SpecificationStr),
                                    2 => GetCarExecution(spec.SpecificationStr),
                                    _ => ""
                                };

                                elem.Attribute("label")!.Value = $"{result ?? ""}\n{info}";
                            }
                            else
                            {
                                elem.Attribute("label")!.Value = $"{result ?? ""}";
                            }
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

    /// <summary>
    /// Получение типа кабины
    /// </summary>
    /// <param name="spec">На вход подаётся спецификация</param>
    private static string GetCarExecution(string spec)
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
    /// Получение исполнения
    /// </summary>
    /// <param name="spec">На вход подаётся спецификация </param>
    private static string GetCabinType(string spec)
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
}