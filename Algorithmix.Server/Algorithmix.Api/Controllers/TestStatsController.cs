using Algorithmix.Api.Core;
using Algorithmix.Api.Core.TestDesign;
using Algorithmix.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Algorithmix.Api.Controllers
{
    [ApiController]
    [Route("api/tests/{testId}/stats")]
    [Authorize(Roles = Roles.Executive)]
    public class TestStatsController : Controller
    {
        private readonly ITestStatsManager _testStatsManager;
        private readonly TestManager _testManager;

        public TestStatsController(ITestStatsManager testStatsManager, TestManager testManager)
        {
            _testStatsManager = testStatsManager;
            _testManager = testManager;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetTestStats(int testId)
        {
            if (!await _testManager.Exists(testId))
                return NotFound();

            var testStats = await _testStatsManager.GetTestStats(testId);
            return Ok(testStats);
        }
    }
}
