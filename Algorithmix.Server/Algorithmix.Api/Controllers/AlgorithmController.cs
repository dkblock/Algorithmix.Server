using Algorithmix.Api.Core;
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
        [Route("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAlgorithm(string id)
        {
            if (!await _algorithmManager.Exists(id))
                return NotFound();

            var algorithm = await _algorithmManager.GetAlgorithm(id);
            return Ok(algorithm);
        }
    }
}
