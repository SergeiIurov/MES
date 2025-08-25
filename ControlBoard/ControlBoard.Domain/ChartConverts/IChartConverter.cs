namespace ControlBoard.Domain.ChartConverts
{
    public interface IChartConverter
    {
        Task<string> Convert(string chartInfo);
    }
}
