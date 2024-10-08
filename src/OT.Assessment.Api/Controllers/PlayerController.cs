using Microsoft.AspNetCore.Mvc;
using OT.Assessment.Api.Contract.Request;
using OT.Assessment.Core;

namespace OT.Assessment.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public PlayerController(
            IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpPost]
        [Route("casinowager")]
        public async Task<ActionResult> PostCasinoWager([FromBody] CasinoWagerRequest wager)
        {
            await _playerService.AddCasinoWager(wager);

            return Ok();
        }

        [HttpGet]
        [Route("{accountId}/wagers")]
        public async Task<ActionResult> GetCasinoWagers(Guid accountId, int? pageSize = null, int? page = null)
        {
            var results = await _playerService.RetrieveCasinoWagers(accountId, pageSize, page);

            return Ok(results);
        }

        [HttpGet]
        [Route("topSpenders")]
        public async Task<ActionResult> GetTopSpenders(int? count = null)
        {
            var results = await _playerService.RetrieveTopSpender(count);

            return Ok(results);
        }
    }
}
