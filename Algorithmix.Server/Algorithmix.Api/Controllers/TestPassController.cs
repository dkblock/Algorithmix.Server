using Algorithmix.Api.Core;
using Algorithmix.Models.Tests;
using Algorithmix.Services;
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
        private readonly UserService _userService;

        public TestPassController(TestPassManager testPassManager, UserTestResultManager userTestResultManager, UserService userService)
        {
            _testPassManager = testPassManager;
            _userTestResultManager = userTestResultManager;
            _userService = userService;
        }

        [HttpGet]
        [Route("pass")]
        public async Task<IActionResult> StartTestPass(int testId, [FromHeader] string authorization)
        {
            var userId = _userService.GetUserIdByAccessToken(authorization);
            var nextQuestion = await _testPassManager.GetNextTestQuestion(null, testId, userId);

            return Ok(nextQuestion);
        }

        [HttpPost]
        [Route("pass/next")]
        public async Task<IActionResult> GetNextTestQuestion(int testId, [FromHeader] string authorization, [FromBody] UserAnswerPayload userAnswerPayload)
        {
            var userId = _userService.GetUserIdByAccessToken(authorization);
            var nextQuestion = await _testPassManager.GetNextTestQuestion(userAnswerPayload, testId, userId);

            return Ok(nextQuestion);
        }

        [HttpPost]
        [Route("pass/previous")]
        public async Task<IActionResult> GetPreviousTestQuestion([FromHeader] string authorization, [FromBody] UserAnswerPayload userAnswerPayload)
        {
            var userId = _userService.GetUserIdByAccessToken(authorization);
            var previousQuestion = await _testPassManager.GetPreviousTestQuestion(userAnswerPayload.QuestionId, userId);

            return Ok(previousQuestion);
        }

        [HttpGet]
        [Route("result")]
        public async Task<IActionResult> GetTestPassResult(int testId, [FromHeader] string authorization)
        {
            var userId = _userService.GetUserIdByAccessToken(authorization);

            if (!await _userTestResultManager.Exists(testId, userId))
                return NotFound();

            var userTestResult = await _userTestResultManager.GetUserTestResult(testId, userId);
            return Ok(userTestResult);
        }
    }
}
