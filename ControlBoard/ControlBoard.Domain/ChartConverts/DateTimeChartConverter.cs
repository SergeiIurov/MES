using ControlBoard.DB.Entities;
using ControlBoard.DB.Repositories;
using ControlBoard.Domain.Services.Abstract;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ControlBoard.Domain.ChartConverts
{
    public class DateTimeChartConverter(IProcessStateAdvService processStateAdvService,
        ILogger<DateTimeChartConverter> logger):IChartConverter
    {
        public async Task<string> Convert(string chartInfo)
        {
            logger.LogInformation($"Запуск метода {nameof(Convert)}.");
        

            XElement root = XElement.Parse(chartInfo);
            var data = root.Descendants("object").Where(e => e.Attribute("sid") != null);
            foreach (XElement elem in data)
            {
                if (elem.Attribute("sid").Value.Equals("last_update"))
                {
                    string updateDateTime = (await processStateAdvService.GetLastUpdateAsync()).ToLocalTime().ToString("HH:mm dd.MM.yyyy");
                    elem.Attribute("label").Value = $"Последнее обновление: {updateDateTime}";
                }
            }
            

            logger.LogInformation($"Выполнение метода {nameof(Convert)} завершено.");
            return root.ToString();
        }
    }
}
