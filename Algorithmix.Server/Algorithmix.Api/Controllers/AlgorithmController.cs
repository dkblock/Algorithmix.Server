using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Algorithmix.Services;

namespace Algorithmix.Server.Controllers
{
    [ApiController]
    [Route("api/algorithms")]
    [Authorize]
    public class AlgorithmController : Controller
    {
        private readonly AlgorithmService _algorithmService;

        public AlgorithmController(AlgorithmService algorithmService)
        {
            _algorithmService = algorithmService;
        }

        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAlgorithms()
        {
            var algorithms = await _algorithmService.GetAllAlgorithms();
            return Ok(algorithms);
        }
    }
}
