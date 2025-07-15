namespace ControlBoard.Domain.Services.Abstract;

public interface IChartConvertService
{
    Task<string> Convert(string from);
}