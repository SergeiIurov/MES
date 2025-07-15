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
            Guid guid = context.ProcessStates.OrderByDescending(s => s).First().GroupId;
            return await context.ProcessStates.Where(s => s.GroupId == guid).ToListAsync();
        }
    }
}