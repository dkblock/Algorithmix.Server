using Algorithmix.Api.Core;
using Algorithmix.Identity;
using Algorithmix.Models.Tests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Algorithmix.Api.Controllers
{
    [ApiController]
    [Route("api/tests/{testId}")]
    [Authorize]
    public class TestPassController : Controller
    {
        private readonly TestPassManager _testPassManager;
        private readonly UserTestResultManager _userTestResultManager;

        public TestPassController(TestPassManager testPassManager, UserTestResultManager userTestResultManager)
        {
            _testPassManager = testPassManager;
            _userTestResultManager = userTestResultManager;
        }

        [HttpGet]
        [Route("pass")]
        public async Task<IActionResult> StartTestPass(int testId)
        {
            var userId = this.GetUser().Id;
            var nextQuestion = await _testPassManager.GetNextTestQuestion(null, testId, userId);

            return Ok(nextQuestion);
        }

        [HttpPost]
        [Route("pass/next")]
        public async Task<IActionResult> GetNextTestQuestion(int testId, [FromBody] UserAnswerPayload userAnswerPayload)
        {
            var userId = this.GetUser().Id;
            var nextQuestion = await _testPassManager.GetNextTestQuestion(userAnswerPayload, testId, userId);

            return Ok(nextQuestion);
        }

        [HttpPost]
        [Route("pass/previous")]
        public async Task<IActionResult> GetPreviousTestQuestion([FromBody] UserAnswerPayload userAnswerPayload)
        {
            var userId = this.GetUser().Id;
            var previousQuestion = await _testPassManager.GetPreviousTestQuestion(userAnswerPayload.QuestionId, userId);

            return Ok(previousQuestion);
        }

        [HttpGet]
        [Route("result")]
        public async Task<IActionResult> GetTestPassResult(int testId)
        {
            var userId = this.GetUser().Id;

            if (!await _userTestResultManager.Exists(testId, userId))
                return NotFound();

            var userTestResult = await _userTestResultManager.GetUserTestResult(testId, userId);
            return Ok(userTestResult);
        }
    }
}
