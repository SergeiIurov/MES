using ControlBoard.DB.Repositories.Abstract;
using ControlBoard.Domain.Services.Abstract;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;

namespace ControlBoard.Domain.Services.Concrete;

/// <summary>
/// Класс занимается преобразованием сырых XML данных доски с целью
/// наполнения информацией всех элементов интерфейса, имеющих атрибут "sid".
/// Значения данного атрибута сопоставляется с id станции. 
/// </summary>
/// <param name="repository"></param>
public class ChartConvertService(
    IProcessStateRepository repository,
    ILogger<ChartConvertService> logger) : IChartConvertService
{
    public async Task<string> Convert(string from)
    {
        Dictionary<int, (string, string)> dict =
            (await repository.GetLastProcessStateAsync()).ToDictionary(s => s.StationId!.Value,
                d => (d.Value, d.ProductType?.Name ?? ""));

        XElement root = XElement.Parse(from);
        var data = root.Descendants("object").Where(e => e.Attribute("sid") != null);

        foreach (XElement elem in data)
        {
            try
            {
                //if (dict.TryGetValue(int.Parse(elem.Attribute("sid")?.Value), out (string, string) result))
                if (dict.TryGetValue(int.TryParse(elem.Attribute("sid")?.Value, out int s) ? s : 0, out (string, string) result))
                {
                    elem.Attribute("label")!.Value = $"{result.Item1}\n{result.Item2}";
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
            }
        }

        return root.ToString();
    }
}