using ControlBoard.DB.Repositories.Abstract;
using ControlBoard.Domain.Services.Abstract;
using Microsoft.Extensions.Logging;
using ControlBoard.DB;
using ControlBoard.Domain.ChartConverts;

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
    DataChartConverter dataChartConverter,
    DisabledChartConverter disabledChartConverter,
    DateTimeChartConverter dateTimeChartConverter,
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
        return await disabledChartConverter.Convert(await dataChartConverter.Convert(await dateTimeChartConverter.Convert(from)));
    }
}