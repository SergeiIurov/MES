using ControlBoard.DB.Entities;
using ControlBoard.DB.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace ControlBoard.DB.Repositories.Concrete
{
    public class ProcessStateRepository(MesDbContext context) : IProcessStateRepository
    {
        public async Task SaveProcessStatesAsync(List<ProcessState> processStates)
        {
            await context.ProcessStates.AddRangeAsync(processStates);
            await context.SaveChangesAsync();
        }

        public async Task<List<ProcessState>> GetLastProcessStateAsync()
        {
            if (context.ProcessStates.Any())
            {
                return await context.ProcessStates.ToListAsync();
            }
            else
            {
                return [];
            }
        }
    }
}