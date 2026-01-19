using DtekShutdowns.Models;
using DtekShutdowns.Services;
using Microsoft.AspNetCore.Mvc;

namespace DtekShutdowns.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShutdownsController : ControllerBase
    {
        private readonly IShutdownScheduleService _shutdownScheduleService;

        public ShutdownsController(IShutdownScheduleService shutdownScheduleService)
        {
            _shutdownScheduleService = shutdownScheduleService;
        }

        [HttpGet("{group}")]
        public async Task<ActionResult<ShutdownScheduleResponse>> Get(string group)
        {
            var result = await _shutdownScheduleService.GetSchedule(group);

            return result.Status switch
            {
                ResponseStatus.Success => result.Result,
                ResponseStatus.Failed => StatusCode(500),
                ResponseStatus.NotFound => NotFound(),
                _ => NotFound(),
            };
        }
    }
}
