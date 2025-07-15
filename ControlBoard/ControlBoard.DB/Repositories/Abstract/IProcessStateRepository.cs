using ControlBoard.DB.Entities;

namespace ControlBoard.DB.Repositories.Abstract
{
    public interface IProcessStateRepository
    {
        Task SaveProcessStatesAsync(List<ProcessState> processStates);
        Task<List<ProcessState>> GetLastProcessStateAsync();
    }
}