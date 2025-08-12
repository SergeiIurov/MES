namespace ControlBoard.Domain.Services.Abstract
{
    public interface IHistoryService
    {
        Task WriteHistoryElementAsync(string jsonInfo);
    }
}
