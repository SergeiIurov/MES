using ControlBoard.DB.Entities;

using ControlBoard.DB.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace ControlBoard.DB.Repositories.Concrete
{
    public class BoardConstructorRepository : Repository<ControlBoardData>, IBoardConstructorRepository
    {
        public BoardConstructorRepository(MesDbContext db) : base(db)
        {
        }

        public async Task<int> UpdateLastDataOrCreateAsync(ControlBoardData data)
        {
            ControlBoardData boardData = await _db.ControlBoardData.OrderDescending().LastOrDefaultAsync();
            if (boardData != null)
            {
                boardData.LastUpdated = DateTime.UtcNow;
                boardData.Data = data.Data;

            }
            else
            {
                boardData = data;
                await _db.ControlBoardData.AddAsync(boardData);
            }

            await _db.SaveChangesAsync();
            return boardData.Id;
        }

        public async Task<int> SaveControlBoardInfoAsync(ControlBoardData data)
        {
            ControlBoardData d = await _db.FindAsync<ControlBoardData>(data.Id);
            if (d != null)
            {
                d.LastUpdated = DateTime.UtcNow;
                d.Data = data.Data;
            }
            else
            {
                d = data;
                await _db.ControlBoardData.AddAsync(d);
            }

            await _db.SaveChangesAsync();
            return d.Id;
        }

        public async Task<ControlBoardData?> GetLastData()
        {
            return await _db.ControlBoardData.OrderDescending().LastOrDefaultAsync();

        }
    }
}
