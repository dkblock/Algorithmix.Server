using Algorithmix.Api.Core;
using Algorithmix.Api.Core.TestPass;
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
        private readonly PublishedTestManager _pubTestManager;
        private readonly TestPassManager _testPassManager;
        private readonly UserTestResultManager _userTestResultManager;

        public TestPassController(
            PublishedTestManager pubTestManager,
            TestPassManager testPassManager,
            UserTestResultManager userTestResultManager)
        {
            _pubTestManager = pubTestManager;
            _testPassManager = testPassManager;
            _userTestResultManager = userTestResultManager;
        }

        [HttpGet]
        [Route("pass")]
        public async Task<IActionResult> StartTestPass(int testId)
        {
            if (!await _pubTestManager.Exists(testId))
                return NotFound();

            var nextQuestion = await _testPassManager.GetNextTestQuestion(null, testId);
            return Ok(nextQuestion);
        }

        [HttpPost]
        [Route("pass/next")]
        public async Task<IActionResult> GetNextTestQuestion(int testId, [FromBody] UserAnswerPayload userAnswerPayload)
        {
            var nextQuestion = await _testPassManager.GetNextTestQuestion(userAnswerPayload, testId);
            return Ok(nextQuestion);
        }

        [HttpPost]
        [Route("pass/previous")]
        public async Task<IActionResult> GetPreviousTestQuestion([FromBody] UserAnswerPayload userAnswerPayload)
        {
            var previousQuestion = await _testPassManager.GetPreviousTestQuestion(userAnswerPayload.QuestionId);
            return Ok(previousQuestion);
        }

        [HttpGet]
        [Route("result")]
        public async Task<IActionResult> GetTestPassResult(int testId)
        {
            if (!await _userTestResultManager.Exists(testId))
                return NotFound();

            var userTestResult = await _userTestResultManager.GetUserTestResult(testId);
            return Ok(userTestResult);
        }
    }
}
