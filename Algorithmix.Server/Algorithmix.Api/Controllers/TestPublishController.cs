using Algorithmix.Api.Core;
using Algorithmix.Api.Validation;
using Algorithmix.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Algorithmix.Api.Controllers
{
    [ApiController]
    [Route("api/tests/{testId}/publish")]
    [Authorize(Roles = Roles.Executive)]
    public class TestPublishController : Controller
    {
        private readonly TestPublishManager _testPublishManager;
        private readonly TestPublishValidator _testPublishValidator;

        public TestPublishController(TestPublishManager testPublishManager, TestPublishValidator testPublishValidator)
        {
            _testPublishManager = testPublishManager;
            _testPublishValidator = testPublishValidator;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> PublishTest(int testId, bool clearTestResults)
        {
            var test = await _testPublishManager.GetPreparedTest(testId);
            var validationResult = _testPublishValidator.Validate(test);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            await _testPublishManager.PublishTest(test, clearTestResults);

            return Ok();
        }
    }
}
