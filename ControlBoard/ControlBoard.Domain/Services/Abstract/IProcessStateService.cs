using ControlBoard.Domain.Dto;

namespace ControlBoard.Domain.Services.Abstract
{
    public interface IProcessStateService
    {
        Task SaveListAsync(List<ProcessStateDto> list);
    }
}
