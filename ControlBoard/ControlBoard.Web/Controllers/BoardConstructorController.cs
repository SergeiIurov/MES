using System.Collections;
using AutoMapper;
using ControlBoard.DB.Entities;
using ControlBoard.Domain.Dto;
using ControlBoard.Domain.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace ControlBoard.Web.Controllers
{
    public class ConstructorData
    {
        public string Data { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class BoardConstructorController(ILogger<BoardConstructorController> logger, IMapper mapper, IStationService stationService,IAreaService areaService, IBoardConstructorService boardConstructorService) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StationDto>>> GetAllStationsDbo()
        {
            try
            {
                logger.LogInformation($"Действие {nameof(GetAllStationsDbo)} запущено.");
                return Ok(mapper.Map<IEnumerable<Station>, IEnumerable<StationDto>>(await stationService.GetStationsAsync()));
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
                return BadRequest(ModelState);
            }
        }
        
        [HttpPost]
        public async Task<ActionResult> UpdateConstructor(ConstructorData data)
        {
            try
            {
                logger.LogInformation($"Действие {nameof(UpdateConstructor)} запущено.");
                return Ok(await boardConstructorService.UpdateLastDataOrCreateAsync(data.Data));
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
                return BadRequest(ModelState);
            }
        }

        [HttpGet("chart")]
        public async Task<ActionResult> GetLastControlBoardData()
        {
            try
            {
                return Ok(await boardConstructorService.GetLastDataAsync());
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
                return BadRequest(ModelState);
            }
        }
    }
}
