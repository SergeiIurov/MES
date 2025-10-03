using AutoMapper;
using ControlBoard.DB.Entities;
using ControlBoard.Domain.Dto;
using ControlBoard.Domain.Services.Abstract;
using ControlBoard.Domain.Services.Abstract.Settings;
using ControlBoard.Domain.Services.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Net;

namespace ControlBoard.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ScanningPointsController(
        ILogger<ScanningPointsController> logger,
        IScanningPointService scanningPointService,
        IMapper mapper) : ControllerBase
    {
        /// <summary>
        /// Возврат списка точек сканирования.
        /// </summary>
        [HttpGet("scanning-points")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<ScanningPoint>>> GetScanningPoints()
        {
            try
            {
                logger.LogInformation($"Действие {nameof(GetScanningPoints)} запущено.");
                return Ok(await scanningPointService.GetScanningPointsAsync());
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Создание точки сканирования.
        /// </summary>
        [HttpPost("scanning-points")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<ScanningPointDto>> AddScanningPoint(ScanningPointDto scanningPoint)
        {
            try
            {
                logger.LogInformation($"Действие {nameof(AddScanningPoint)} запущено.");
                OkObjectResult result = Ok(mapper.Map<ScanningPoint, ScanningPointDto>(await scanningPointService.AddScanningPoiintAsync(scanningPoint)));
                return result;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Удаление существующей точки сканирования.
        /// </summary>
        [HttpDelete("scanning-points/{id}")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<ActionResult> DeleteScanningPoint(int id)
        {
            try
            {
                logger.LogInformation($"Действие {nameof(DeleteScanningPoint)} запущено.");
                await scanningPointService.DeleteScanningPointAsync(id);
                return NoContent();
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Обновление существующей точки сканирования.
        /// </summary>
        [HttpPut("scanning-points")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<ActionResult<ScanningPointDto>> UpdateScanningPoint(ScanningPointDto scanningPoint)
        {
            try
            {
                logger.LogInformation($"Действие {nameof(UpdateScanningPoint)} запущено.");
                OkObjectResult result = Ok(mapper.Map<ScanningPoint, ScanningPointDto>(await scanningPointService.UpdateScanningPointAsync(scanningPoint)));
                return result;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
                return BadRequest(ModelState);
            }
        }

    }
}