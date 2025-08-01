using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlBoard.Domain.Services.Abstract
{
    public interface IHistoryService
    {
        Task WriteHistoryElementAsync(string jsonInfo);
    }
}
