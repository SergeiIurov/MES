using ControlBoard.DB.Entities;

namespace ControlBoard.DB.Repositories.Abstract
{
    public interface IProcessStateRepository
    {
        Task SaveProcessStates(List<ProcessState> processStates);
    }
}
