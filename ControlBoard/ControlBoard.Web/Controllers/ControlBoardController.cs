using AutoMapper;
using ControlBoard.DB;
using ControlBoard.DB.Entities;
using ControlBoard.Domain.Dto;
using ControlBoard.Domain.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ControlBoard.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ControlBoardController(IHubContext<MesHub> hub, IWebHostEnvironment env, IProcessStateService processStateService) : Controller
    {
        [HttpPost]
        //[Authorize]
        public async Task UploadPicture(IFormFile file)
        {

            using FileStream fs = System.IO.File.Create($"{env.WebRootPath}/files/board.jpg");
            file.CopyTo(fs);
            await hub.Clients.All.SendAsync("notifyAll");
        }

        [HttpPost("excel-data")]
        //[Authorize]
        public async Task UploadData(List<ProcessStateDto> list)
        {
           await processStateService.SaveListAsync(list);
        }

        [HttpGet("image")]
        //[Authorize]
        public string GetPicture()
        {
            using FileStream fs = System.IO.File.OpenRead($"{env.WebRootPath}/files/board.jpg");
            MemoryStream ms = new MemoryStream();
            fs.CopyTo(ms);
            string str = Convert.ToBase64String(ms.ToArray());
            return $"data:image/image/jpg;base64,{str}";
        }
    }
}