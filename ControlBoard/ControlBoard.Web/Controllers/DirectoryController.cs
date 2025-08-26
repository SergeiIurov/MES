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
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DirectoryController(
        ILogger<ControlBoardController> logger,
        IAreaService areaService,
        IProductTypeService productTypeService,
        IStationService stationService,
        ICarExecutionService carExecutionService,
        IHubContext<MesHub> hub,
        IMapper mapper) : ControllerBase
    {
        /// <summary>
        /// Возврат списка участков с содержищимися в них станциями.
        /// </summary>
        [HttpGet("areas")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<AreaDto>>> GetAreas()
        {
            try
            {
                logger.LogInformation($"Действие {nameof(GetAreas)} запущено.");
                return Ok(mapper.Map<List<Area>, IEnumerable<AreaDto>>(await areaService.GetAreasAsync()));
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Создание нового участка.
        /// </summary>
        [HttpPost("areas")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<AreaDto>> AddArea(AreaDto area)
        {
            try
            {
                logger.LogInformation($"Действие {nameof(AddArea)} запущено.");
                OkObjectResult result = Ok(mapper.Map<Area, AreaDto>(await areaService.AddAreaAsync(area)));
                await hub.Clients.All.SendAsync("сontrolBoardInfoUpdated");
                return result;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
                return BadRequest(ModelState);
            }
        }


        /// <summary>
        /// Удаление существующего участка.
        /// </summary>
        [HttpDelete("areas/{id}")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<ActionResult> DeleteArea(int id)
        {
            try
            {
                logger.LogInformation($"Действие {nameof(DeleteArea)} запущено.");
                await areaService.DeleteAreaAxync(id);
                await hub.Clients.All.SendAsync("сontrolBoardInfoUpdated");
                return NoContent();
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Обновление существующего участка.
        /// </summary>
        [HttpPut("areas")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<ActionResult<AreaDto>> UpdateArea(AreaDto area)
        {
            try
            {
                logger.LogInformation($"Действие {nameof(UpdateArea)} запущено.");
                OkObjectResult result = Ok(mapper.Map<Area, AreaDto>(await areaService.UpdateAreaAsync(area)));
                await hub.Clients.All.SendAsync("сontrolBoardInfoUpdated");
                return result;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Обновление цвета блокировки участка.
        /// </summary>
        [HttpPut("areas/set-disabled-color")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task UpdateDisabledColor(AreaColorInputData data)
        {
            try
            {
                logger.LogInformation($"Действие {nameof(UpdateDisabledColor)} запущено.");
                await areaService.SetDisabledColorAsync(data.Id, data.Color);
                await hub.Clients.All.SendAsync("сontrolBoardInfoUpdated");
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
            }
        }

        /// <summary>
        /// Возврат списка станций.
        /// </summary>
        [HttpGet("stations")]
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
        /// Проверка уникальности ID(CharElementId) для станции.
        /// </summary>
        [HttpGet("stations/isfree/{id}/{chartElementId}")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> IsFree(int id, int chartElementId)
        {
            try
            {
                logger.LogInformation($"Действие {nameof(IsFree)} запущено.");
                return Ok(await stationService.IsFreeAsync(id, chartElementId));
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Проверка корректности диапазона.
        /// </summary>
        [HttpGet("stations/isinrange/{id}/{areaId}/{chartElementId}")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> IsInRangeAsync(int id, int areaId, int chartElementId)
        {
            try
            {
                logger.LogInformation($"Действие {nameof(IsInRangeAsync)} запущено.");
                return Ok(await stationService.IsInRangeAsync(id, areaId, chartElementId));
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Создание новой станции.
        /// </summary>
        [HttpPost("stations")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<StationDto>> AddStation(StationDto station)
        {
            try
            {
                logger.LogInformation($"Действие {nameof(AddStation)} запущено.");
                OkObjectResult result = Ok(mapper.Map<Station, StationDto>(await stationService.AddStationAsync(station)));
                await hub.Clients.All.SendAsync("сontrolBoardInfoUpdated");
                return result;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
                return BadRequest(ModelState);
            }
        }


        /// <summary>
        /// Удаление существующей станции.
        /// </summary>
        [HttpDelete("stations/{id}")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<ActionResult> DeleteStation(int id)
        {
            try
            {
                logger.LogInformation($"Действие {nameof(DeleteStation)} запущено.");
                await stationService.DeleteStationAxync(id);
                await hub.Clients.All.SendAsync("сontrolBoardInfoUpdated");
                return NoContent();
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
                return BadRequest(e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

        /// <summary>
        /// Обновление существующей станции.
        /// </summary>
        [HttpPut("stations")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<ActionResult<StationDto>> UpdateStation(StationDto station)
        {
            try
            {
                logger.LogInformation($"Действие {nameof(UpdateStation)} запущено.");

                OkObjectResult result = Ok(mapper.Map<Station, StationDto>(await stationService.UpdateStationAsync(station)));
                await hub.Clients.All.SendAsync("сontrolBoardInfoUpdated");
                return result;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
                return BadRequest(ModelState);
            }
        }


        /// <summary>
        /// Возврат списка типа продуктов.
        /// </summary>
        [HttpGet("product-types")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<ProductTypeDto>>> GetAllProductTypesDbo()
        {
            try
            {
                logger.LogInformation($"Действие {nameof(GetAllProductTypesDbo)} запущено.");
                return Ok(mapper.Map<IEnumerable<ProductType>, IEnumerable<ProductTypeDto>>(
                    await productTypeService.GetProductTypesAsync()));
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Возврат списка исполнения автомобилей.
        /// </summary>
        [HttpGet("carexecutions")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<CarExecutionDto>>> GetCarExecutions()
        {
            try
            {
                logger.LogInformation($"Действие {nameof(GetCarExecutions)} запущено.");
                return Ok(mapper.Map<List<CarExecution>, IEnumerable<CarExecutionDto>>(
                    await carExecutionService.GetCarExecutionsAsync()));
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Создание нового исполнения автомобилей.
        /// </summary>
        [HttpPost("carexecutions")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<CarExecutionDto>> AddCarExecution(CarExecutionDto carExecution)
        {
            try
            {
                logger.LogInformation($"Действие {nameof(AddCarExecution)} запущено.");
                OkObjectResult okObjectResult =
                    Ok(mapper.Map<CarExecution, CarExecutionDto>(
                        await carExecutionService.AddCarExecutionAsync(carExecution)));
                await hub.Clients.All.SendAsync("сontrolBoardInfoUpdated");
                return okObjectResult;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
                return BadRequest(ModelState);
            }
        }


        /// <summary>
        /// Удаление существующего исполнения.
        /// </summary>
        [HttpDelete("carexecutions/{id}")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<ActionResult> DeleteCarExecution(int id)
        {
            try
            {
                logger.LogInformation($"Действие {nameof(DeleteCarExecution)} запущено.");
                await carExecutionService.DeleteExecutionAsync(id);
                await hub.Clients.All.SendAsync("сontrolBoardInfoUpdated");
                return NoContent();
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Обновление существующего исполнения.
        /// </summary>
        [HttpPut("carexecutions")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<ActionResult<CarExecutionDto>> UpdateCarExecution(CarExecutionDto carExecutionDto)
        {
            try
            {
                logger.LogInformation($"Действие {nameof(UpdateCarExecution)} запущено.");
                OkObjectResult okObjectResult = Ok(mapper.Map<CarExecution, CarExecutionDto>(
                    await carExecutionService.UpdateCarExecutionAsync(carExecutionDto)));
                await hub.Clients.All.SendAsync("сontrolBoardInfoUpdated");
                return okObjectResult;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
                return BadRequest(ModelState);
            }
        }

        public class AreaColorInputData
        {
            public int Id { get; set; }
            public string Color { get; set; }
        }
    }
}