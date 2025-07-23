using System.Net.Mime;
using ControlBoard.Domain.Dto;
using ControlBoard.Domain.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;

namespace ControlBoard.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ControlBoardAdvController(
    IHubContext<MesHub> hub,
    IProcessStateAdvService processStateAdvService,
    IBoardConstructorService boardConstructorService,
    IChartConvertService chartConvertService,
    ILogger<ControlBoardController> logger) : ControllerBase
{

    /// <summary>
    /// Сохранение нового состояния доски контроля производства.
    /// </summary>
    [HttpPost]
    public async Task SaveStateInfo(List<ProcessStateAdvDto> list)
    {
        try
        {
            logger.LogInformation($"Действие {nameof(SaveStateInfo)} запущено.");
            await processStateAdvService.SaveListAsync(list);
            await hub.Clients.All.SendAsync("сontrolBoardInfoUpdated");
            logger.LogInformation($"Действие {nameof(SaveStateInfo)} завершено.");
        }
        catch (Exception e)
        {
            logger.LogError(e.Message, e);
        }
    }

    /// <summary>
    /// Получение нового состояния доски контроля производства для дальнейшей визуализации.
    /// </summary>
    [HttpGet("chart")]
    [AllowAnonymous]
    public async Task<ActionResult> GetLastControlBoardData()
    {
        try
        {
            return Ok(await chartConvertService.Convert(await boardConstructorService.GetLastDataAsync()));
        }
        catch (Exception e)
        {
            logger.LogError(e.Message, e);
            return BadRequest(ModelState);
        }
    }
}
