using System.Net;
using AutoMapper;
using ControlBoard.DB.Entities;
using ControlBoard.Domain.Dto;
using ControlBoard.Domain.Services.Abstract;
using ControlBoard.Domain.Services.Concrete;
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
        /// Возврат списка зон с содержищимися в них станциями.
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