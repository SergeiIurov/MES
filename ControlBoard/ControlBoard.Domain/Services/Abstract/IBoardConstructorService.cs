namespace ControlBoard.Domain.Services.Abstract
{
    public interface IBoardConstructorService
    {
        Task<int> UpdateLastDataOrCreateAsync(string chart);
        Task<string> GetLastDataAsync();
    }
}
