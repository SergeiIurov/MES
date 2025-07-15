using System.Xml.Linq;
using ControlBoard.DB.Repositories.Abstract;
using ControlBoard.Domain.Services.Abstract;

namespace ControlBoard.Domain.Services.Concrete;

public class ChartConvertService(IProcessStateRepository repository) : IChartConvertService
{
    public async Task<string> Convert(string from)
    {
        Dictionary<int, string> dict =
            (await repository.GetLastProcessStateAsync()).ToDictionary(s => s.StationId, d => d.Value);

        XElement root = XElement.Parse(from);
        var data = root.Descendants("object").Where(e => e.Attribute("sid") != null);

        foreach (XElement elem in data)
        {
            if (dict.TryGetValue(int.Parse(elem.Attribute("sid").Value), out string result))
            {
                elem.Attribute("label").Value = result;
            }
        }

        return root.ToString();
    }
}