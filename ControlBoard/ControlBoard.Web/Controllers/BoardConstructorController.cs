using AutoMapper;
using ControlBoard.DB.Entities;
using ControlBoard.Domain.Dto;
using ControlBoard.Domain.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Net;

namespace ControlBoard.Web.Controllers
{
    public class ConstructorData
    {
        public string Data { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BoardConstructorController(
        IHubContext<MesHub> hub,
        ILogger<BoardConstructorController> logger,
        IMapper mapper,
        IStationService stationService,
        IBoardConstructorService boardConstructorService
    ) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<StationDto>>> GetAllStationsDbo()
        {
            try
            {
                logger.LogInformation($"Действие {nameof(GetAllStationsDbo)} запущено.");
                return Ok(mapper.Map<IEnumerable<Station>, IEnumerable<StationDto>>(
                    await stationService.GetStationsAsync()));
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
                return BadRequest(ModelState);
            }
        }


        /// <summary>
        /// Обновление текущего состояния конструктора
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateConstructor(ConstructorData data)
        {
            try
            {
                logger.LogInformation($"Действие {nameof(UpdateConstructor)} запущено.");
                int id = await boardConstructorService.UpdateLastDataOrCreateAsync(data.Data);
                await hub.Clients.All.SendAsync("сontrolBoardInfoUpdated");
                return Ok(id);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Получение текущего состояния конструктора для дальнейшей визуализации.
        /// </summary>
        [HttpGet("chart")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetLastControlBoardData()
        {
            try
            {
                return Ok(await boardConstructorService.GetLastDataAsync());
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
                return BadRequest(ModelState);
            }
        }
    }
}