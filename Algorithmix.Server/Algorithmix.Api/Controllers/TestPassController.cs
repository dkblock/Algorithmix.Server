using Algorithmix.Api.Managers;
using Algorithmix.Models.Tests;
using Algorithmix.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Algorithmix.Api.Controllers
{
    [ApiController]
    [Route("api/tests/{testId}/pass")]
    [Authorize]
    public class TestPassController : Controller
    {
        private readonly TestPassManager _testPassManager;
        private readonly UserService _userService;

        public TestPassController(TestPassManager testPassManager, UserService userService)
        {
            _testPassManager = testPassManager;
            _userService = userService;
        }

        [HttpPost]
        [Route("next")]
        public async Task<IActionResult> GetNextTestQuestion(int testId, [FromHeader] string authorization, [FromBody] UserAnswerPayload userAnswerPayload)
        {
            var userId = _userService.GetUserIdByAccessToken(authorization);
            var nextQuestion = await _testPassManager.GetNextTestQuestion(userAnswerPayload, userId, testId);

            return Ok(nextQuestion);
        }

        [HttpPost]
        [Route("previous")]
        public async Task<IActionResult> GetPreviousTestQuestion([FromHeader] string authorization, [FromBody] UserAnswerPayload userAnswerPayload)
        {
            var userId = _userService.GetUserIdByAccessToken(authorization);
            var previousQuestion = await _testPassManager.GetPreviousTestQuestion(userAnswerPayload.QuestionId, userId);

            return Ok(previousQuestion);
        }
    }
}
