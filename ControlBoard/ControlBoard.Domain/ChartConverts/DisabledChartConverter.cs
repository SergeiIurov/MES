using System.Text.RegularExpressions;
using System.Xml.Linq;
using ControlBoard.DB;
using ControlBoard.DB.Entities;
using ControlBoard.DB.Repositories.Abstract;
using ControlBoard.Domain.Services.Abstract;
using ControlBoard.Domain.Services.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ControlBoard.Domain.ChartConverts
{
    /// <summary>
    /// Обработка визуализации графика при отключении участков
    /// </summary>
    public class DisabledChartConverter(IProcessStateRepository repository,
        IProcessStateAdvService processStateAdvService,
        MesDbContext context,
        IChartServices chartService,
        ILogger<ChartConvertService> logger) : IChartConverter
    {
        public async Task<string> Convert(string chartInfo)
        {
            logger.LogInformation($"Запуск метода {nameof(Convert)}.");
            Dictionary<int, (bool, string)> dict;
            List<ProcessState> processStates = await repository.GetLastProcessStateAsync();


            dict = context.Stations.Include(s => s.Area).ToDictionary(s => s.ChartElementId, s => (s.Area.IsDisabled, s.Area.DisabledColor));


            XElement root = XElement.Parse(chartInfo);
            var data = root.Descendants("object").Where(e => e.Attribute("sid") != null);

            string pattern = @"fillColor=(none|#\w{6});";

            foreach (XElement elem in data)
            {
                try
                {
                    if (dict.TryGetValue(int.TryParse(elem.Attribute("sid")?.Value, out int s) ? s : 0, out (bool, string) disabledInfo))
                    {
                        if (disabledInfo.Item1)
                        {

                            var d = elem.Element("mxCell").Attribute("style").Value;

                            if (Regex.IsMatch(d, pattern))
                            {
                                elem.Element("mxCell").Attribute("style").Value = Regex.Replace(d, pattern, $"fillColor={disabledInfo.Item2};");
                            }
                            else
                            {
                                elem.Element("mxCell").Attribute("style").Value = (d += $"fillColor={disabledInfo.Item2};");
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
    }
}