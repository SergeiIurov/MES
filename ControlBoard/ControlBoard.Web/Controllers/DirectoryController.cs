using AutoMapper;
using ControlBoard.DB.Entities;
using ControlBoard.Domain.Dto;
using ControlBoard.Domain.Services.Abstract;
using ControlBoard.Domain.Services.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace ControlBoard.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DirectoryController(ILogger<ControlBoardController> logger, IAreaService areaService, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AreaDto>>> GetAreas()
        {
            try
            {
                logger.LogInformation($"Действие {nameof(GetAreas)} запущено.");
                return Ok(mapper.Map<IEnumerable<Area>,IEnumerable<AreaDto>>(await areaService.GetAreasAsync()));
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
                return BadRequest(ModelState);
            }
        }
    }
}