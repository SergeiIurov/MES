namespace ControlBoard.Domain.Services.Abstract.Settings
{
    public interface ISettingsService
    {
        public Task<int> GetLineCountAsync();
    }
}
