using ControlBoard.Domain.Dto;
using ControlBoard.Domain.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ControlBoard.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ControlBoardAdvController(
    IHubContext<MesHub> hub,
    IProcessStateAdvService processStateAdvService,
    ILogger<ControlBoardController> logger) : ControllerBase
{
    [HttpPost]
    public async Task SaveStateInfo(List<ProcessStateAdvDto> list)
    {
        try
        {
            logger.LogInformation($"Действие {nameof(SaveStateInfo)} запущено.");
            //await processStateAdvService.SaveListAsync(list);
            await hub.Clients.All.SendAsync("сontrolBoardInfoUpdated");
            logger.LogInformation($"Действие {nameof(SaveStateInfo)} завершено.");
        }
        catch (Exception e)
        {
            logger.LogError(e.Message, e);
        }
    }
}