using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VisualAlgorithms.Services;

namespace VisualAlgorithms.Server.Controllers
{
    [ApiController]
    [Route("users")]
    public class TestsController : Controller
    {
        private readonly TestsService _testsService;

        public TestsController(TestsService testsService)
        {
            _testsService = testsService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetTests()
        {
            var tests = await _testsService.GetAllTests();
            return Ok();
        }
    }
}
