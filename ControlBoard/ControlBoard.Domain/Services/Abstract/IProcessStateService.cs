using ControlBoard.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlBoard.Domain.Services.Abstract
{
    public interface IProcessStateService
    {
        Task SaveListAsync(List<ProcessStateDto> list);
    }
}
