using System.Net;
using AutoMapper;
using ControlBoard.DB.Entities;
using ControlBoard.Domain.Dto;
using ControlBoard.Domain.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        IMapper mapper) : ControllerBase
    {
        /// <summary>
        /// Возврат списка участков с содержищимися в них станциями.
        /// </summary>
        [HttpGet("areas")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<AreaDto>>> GetAreas()
        {
            try
            {
                logger.LogInformation($"Действие {nameof(GetAreas)} запущено.");
                return Ok(mapper.Map<IEnumerable<Area>, IEnumerable<AreaDto>>(await areaService.GetAreasAsync()));
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
                return Ok(mapper.Map<Area, AreaDto>(await areaService.AddAreaAsync(area)));
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
                return Ok(mapper.Map<Area, AreaDto>(await areaService.UpdateAreaAsync(area)));
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
                return BadRequest(ModelState);
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
                return Ok(mapper.Map<Station, StationDto>(await stationService.AddStationAsync(station)));
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
                return Ok(mapper.Map<Station, StationDto>(await stationService.UpdateStationAsync(station)));
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
    }
}