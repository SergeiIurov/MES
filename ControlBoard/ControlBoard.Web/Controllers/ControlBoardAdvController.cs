using AutoMapper;
using ControlBoard.DB.Entities;
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
    IMapper mapper,
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
            await processStateAdvService.SaveListAsync(list, HttpContext.User.Identity.Name);
            await hub.Clients.All.SendAsync("сontrolBoardInfoUpdated");
            logger.LogInformation($"Действие {nameof(SaveStateInfo)} завершено.");
        }
        catch (Exception e)
        {
            logger.LogError(e.Message, e);
        }
    }

    /// <summary>
    /// Получение состояния доски контроля производства.
    /// </summary>
    [HttpGet("state")]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<AreaDto>>> GetState()
    {
        try
        {
            logger.LogInformation($"Действие {nameof(GetState)} запущено.");
            return Ok(mapper.Map<List<ProcessState>, List<ProcessStateAdvDto>>(await processStateAdvService.GetProcessStatesAsync()));
        }
        catch (Exception e)
        {
            logger.LogError(e.Message, e);
            return BadRequest(ModelState);
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


    /// <summary>
    /// Загрузка файла
    /// </summary>
    [HttpPost("upload")]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<ProductTypeDto>>> UploadData(IFormFile file)
    {
        try
        {
            List<(string, string, string, string)> data = new List<(string, string, string, string)>();
            logger.LogInformation($"Действие {nameof(UploadData)} запущено.");
            StreamReader reader = new StreamReader(file.OpenReadStream());
            reader.BaseStream.Position = 0;
            string? line = null;
            while ((line = reader.ReadLine()) != null)
            {
                string[] mas = line.Split(';');
                if (mas.Length == 4)
                {
                    data.Add((mas[0].PadLeft(3, '0'), mas[1], mas[2], mas[3]));
                }
            }

            await processStateAdvService.SaveSpecificationAsync(data);

            return Ok();
        }
        catch (Exception e)
        {
            logger.LogError(e.Message, e);
            return BadRequest(ModelState);
        }
    }


    /// <summary>
    /// Возврат списка участков с содержищимися в них станциями.
    /// </summary>
    [HttpGet("specifications")]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<List<AreaDto>>> GetAreas()
    {
        try
        {
            logger.LogInformation($"Действие {nameof(GetAreas)} запущено.");
            return Ok(mapper.Map<List<Specification>, IEnumerable<SpecificationDto>>(await processStateAdvService.GetSpecifications()));
        }
        catch (Exception e)
        {
            logger.LogError(e.Message, e);
            return BadRequest(ModelState);
        }
    }


    /// <summary>
    /// Изменение статуса активности участка.
    /// </summary>
    [HttpPost("change_disabled_status")]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<bool>> ChangeDisabledStatus(AreaDto areaDto)
    {
        try
        {
            logger.LogInformation($"Действие {nameof(ChangeDisabledStatus)} запущено.");
            OkObjectResult result = Ok(processStateAdvService.ChangeDisabledStatus(areaDto));
            await hub.Clients.All.SendAsync("сontrolBoardInfoUpdated");
            return result;
        }
        catch (Exception e)
        {
            logger.LogError(e.Message, e);
            return BadRequest(ModelState);
        }
    }
}
