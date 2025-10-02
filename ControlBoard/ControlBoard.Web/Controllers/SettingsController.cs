using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ControlBoard.Domain.Services.Abstract.Settings;

namespace ControlBoard.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SettingsController(
        ILogger<ControlBoardController> logger,
        ISettingsService settingsService) : ControllerBase
    {
        /// <summary>
        /// Возврат списка участков с содержищимися в них станциями.
        /// </summary>
        [HttpGet("line-count")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<int>> GetLineCount()
        {
            try
            {
                logger.LogInformation($"Действие {nameof(GetLineCount)} запущено.");
                return Ok(await settingsService.GetLineCountAsync());
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
                return BadRequest(ModelState);
            }
        }

    }
}