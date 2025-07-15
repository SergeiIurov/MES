using ControlBoard.DB.Entities;
using ControlBoard.DB.Repositories.Abstract;

namespace ControlBoard.DB.Repositories.Concrete
{
    public class ProcessStateRepository(MesDbContext context) : IProcessStateRepository
    {
        public async Task SaveProcessStates(List<ProcessState> processStates)
        {
            await context.ProcessStates.AddRangeAsync(processStates);
            await context.SaveChangesAsync();
        }
    }
}
