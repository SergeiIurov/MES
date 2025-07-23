using ControlBoard.Domain.Dto;
using ControlBoard.Domain.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Net;

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
    [ProducesResponseType((int)HttpStatusCode.OK)]
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
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetLastControlBoardData()
    {
        try
        {
            logger.LogInformation($"Действие {nameof(GetLastControlBoardData)} запущено.");
            return Ok(await chartConvertService.Convert(await boardConstructorService.GetLastDataAsync()));
        }
        catch (Exception e)
        {
            logger.LogError(e.Message, e);
            return BadRequest(ModelState);
        }
    }
}
