using AutoMapper;
using ControlBoard.DB.Entities;
using ControlBoard.Domain.Dto;
using ControlBoard.Domain.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace ControlBoard.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BoardConstructorController(ILogger<BoardConstructorController> logger, IMapper mapper, IStationService stationService):ControllerBase
    {

        [HttpGet]
        public async Task<IEnumerable<StationDto>> GetAllStationsDbo()
        {
            return mapper.Map<IEnumerable<Station>, IEnumerable<StationDto>>(await stationService.GetStationsAsync());
        }
    }
}
