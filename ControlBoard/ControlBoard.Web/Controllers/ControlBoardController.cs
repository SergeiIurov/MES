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
    public class ControlBoardController(IHubContext<MesHub> hub, IWebHostEnvironment env, IProcessStateService processStateService, ILogger<ControlBoardController> logger) : ControllerBase
    {
        /// <summary>
        /// Загрузка скриншота доски контроля производства
        /// </summary>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task UploadJpgFile(IFormFile file)
        {
            try
            {
                logger.LogInformation($"Действие {nameof(UploadJpgFile)} запущено.");
                using FileStream fs = System.IO.File.Create($"{env.WebRootPath}/files/board.jpg");
                file.CopyTo(fs);
                await hub.Clients.All.SendAsync("notifyAll");
                logger.LogInformation($"Действие {nameof(UploadJpgFile)} завершено.");
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
            }
        }

        /// <summary>
        /// Загрузка CSV файла с данными текущего состояния доски контроля производства.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost("csv-data")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task UploadCsvFile(List<ProcessStateDto> list)
        {
            try
            {
                logger.LogInformation($"Действие {nameof(UploadCsvFile)} запущено.");
                await processStateService.SaveListAsync(list);
                logger.LogInformation($"Действие {nameof(UploadCsvFile)} завершено.");
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
            }

        }

        /// <summary>
        /// Возврат скриншота с текущим состоянием доски контроля производства.
        /// </summary>
        /// <returns></returns>
        [HttpGet("image")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public string GetPicture()
        {
            try
            {
                logger.LogInformation($"Действие {nameof(GetPicture)} запущено.");
                using FileStream fs = System.IO.File.OpenRead($"{env.WebRootPath}/files/board.jpg");
                MemoryStream ms = new MemoryStream();
                fs.CopyTo(ms);
                string str = Convert.ToBase64String(ms.ToArray());
                logger.LogInformation($"Действие {nameof(GetPicture)} завершено. Готова отправка скриншота.");
                return $"data:image/image/jpg;base64,{str}";
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
                throw;
            }
        }
    }
}