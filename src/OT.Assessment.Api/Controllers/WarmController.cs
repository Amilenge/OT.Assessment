using Microsoft.AspNetCore.Mvc;
using OT.Assessment.Core;

namespace OT.Assessment.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class WarmController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public WarmController(
            IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet]
        public async Task<ActionResult> Warm()
        {
            return Ok();
        }
    }
}
