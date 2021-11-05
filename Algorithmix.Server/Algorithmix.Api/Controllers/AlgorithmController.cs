using Algorithmix.Api.Core;
using Algorithmix.Common.Constants;
using Algorithmix.Models.Algorithms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Algorithmix.Server.Controllers
{
    [ApiController]
    [Route("api/algorithms")]
    [Authorize]
    public class AlgorithmController : Controller
    {
        private readonly AlgorithmManager _algorithmManager;

        public AlgorithmController(AlgorithmManager algorithmManager)
        {
            _algorithmManager = algorithmManager;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = Roles.Administrator)]
        public async Task<IActionResult> CreateAlgorithm([FromBody] AlgorithmPayload algorithmPayload)
        {
            var createdAlgorithm = await _algorithmManager.CreateAlgorithm(algorithmPayload);
            return CreatedAtAction(nameof(GetAlgorithm), new { algorithmId = createdAlgorithm.Id }, createdAlgorithm);
        }

        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAlgorithms(string searchText = "")
        {
            var query = new AlgorithmQuery(searchText);
            var algorithms = await _algorithmManager.GetAlgorithms(query);

            return Ok(algorithms);
        }

        [HttpGet]
        [Route("{algorithmId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAlgorithm(string algorithmId)
        {
            if (!await _algorithmManager.Exists(algorithmId))
                return NotFound();

            var algorithm = await _algorithmManager.GetAlgorithm(algorithmId);
            return Ok(algorithm);
        }
    }
}
