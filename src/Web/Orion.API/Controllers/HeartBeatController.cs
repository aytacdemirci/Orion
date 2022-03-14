using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Orion.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeartBeatController : ControllerBase
    {
        [HttpGet]
        [Route("ping")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public Task<ActionResult<bool>> PingAsync()
        {
            return Task.FromResult<ActionResult<bool>>(Ok(true));
        }
    }
}
