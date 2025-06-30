using ControlBoard.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ControlBoard.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ControlBoardController(IHubContext<MesHub> hub, IWebHostEnvironment env) : Controller
    {
        [HttpPost]
        //[Authorize]
        public async Task UploadPicture(IFormFile file)
        {

            using FileStream fs = System.IO.File.Create($"{env.WebRootPath}/files/board.jpg");
            file.CopyTo(fs);
            hub.Clients.All.SendAsync("notifyAll");
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