using ControlBoard.DB.Entities;
using ControlBoard.DB.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using System;

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
            ProcessState? ps = await context.ProcessStates.OrderByDescending(s => s).FirstOrDefaultAsync();
            if (ps != null)
            {
                return await context.ProcessStates.Where(s => s.GroupId == ps.GroupId).ToListAsync();
            }
            else
            {
                return [];
            }
        }
    }
}