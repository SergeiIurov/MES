using ControlBoard.DB.Entities;

namespace ControlBoard.DB.Repositories.Abstract
{
    public interface IBoardConstructorRepository
    {
        Task<int> SaveControlBoardInfoAsync(ControlBoardData data);
        Task<int> UpdateLastDataOrCreateAsync(ControlBoardData data);
    }
}
