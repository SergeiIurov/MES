using ControlBoard.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlBoard.Domain.Services.Abstract
{
    public interface IBoardConstructorService
    {
        Task<int> UpdateLastDataOrCreateAsync(string chart);
        Task<string> GetLastDataAsync();
    }
}
