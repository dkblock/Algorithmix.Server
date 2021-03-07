using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VisualAlgorithms.Common.Constants;
using VisualAlgorithms.Services;

namespace VisualAlgorithms.Server.Controllers
{
    [ApiController]
    [Route("api/tests")]
    public class TestsController : Controller
    {
        private readonly TestsService _testsService;

        public TestsController(TestsService testsService)
        {
            _testsService = testsService;
        }

        [HttpGet]
        [Route("")]
        [Authorize]
        public async Task<IActionResult> GetTests()
        {
            var tests = await _testsService.GetTests();
            return Ok(tests);
        }
    }
}
