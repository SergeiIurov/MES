using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ControlBoard.DB.Entities;
using ControlBoard.Domain.Services.Abstract.Settings;

namespace ControlBoard.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ScanningPointsController(
        ILogger<ScanningPointsController> logger,
        IScanningPointService scanningPointService) : ControllerBase
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

    }
}