using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VisualAlgorithms.Services;

namespace VisualAlgorithms.Server.Controllers
{
    [ApiController]
    [Route("api/algorithms")]
    [Authorize]
    public class AlgorithmsController : Controller
    {
        private readonly AlgorithmsService _algorithmsService;

        public AlgorithmsController(AlgorithmsService algorithmsService)
        {
            _algorithmsService = algorithmsService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllAlgorithms()
        {
            var algorithms = await _algorithmsService.GetAllAlgorithms();
            return Ok(algorithms);
        }
    }
}
