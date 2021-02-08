using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using VisualAlgorithms.Models;
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
            return Ok();
        }
    }
}
