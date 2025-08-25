using System.Xml.Linq;
using ControlBoard.DB;
using ControlBoard.DB.Entities;
using ControlBoard.DB.Entities.Enums;
using ControlBoard.DB.Repositories.Abstract;
using ControlBoard.Domain.Services.Abstract;
using ControlBoard.Domain.Services.Concrete;
using Microsoft.Extensions.Logging;

namespace ControlBoard.Domain.ChartConverts
{
    public class DataChartConverter(IProcessStateRepository repository,
        IProcessStateAdvService processStateAdvService,
        MesDbContext context,
        IChartServices chartService,
        ILogger<ChartConvertService> logger) :IChartConverter
    {
        public async Task<string> Convert(string chartInfo)
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

                XElement root = XElement.Parse(chartInfo);
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
                                    string? info = await chartService.GetProductTypeAsync(spec.SpecificationStr, result.Item2.Value);

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

            return chartInfo;
        }
    }
}
