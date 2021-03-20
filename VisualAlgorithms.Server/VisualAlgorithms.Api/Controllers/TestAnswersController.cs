using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using VisualAlgorithms.Api.Managers;
using VisualAlgorithms.Api.Validation;
using VisualAlgorithms.Common.Constants;
using VisualAlgorithms.Models.Tests;

namespace VisualAlgorithms.Api.Controllers
{
    [ApiController]
    [Route("api/tests/{testId}/questions/{questionId}/answers")]
    [Authorize]
    public class TestAnswersController : Controller
    {
        private readonly TestAnswersManager _answersManager;
        private readonly TestQuestionsManager _questionsManager;
        private readonly TestAnswersValidator _answersValidator;

        public TestAnswersController(
            TestAnswersManager answersManager,
            TestQuestionsManager questionsManager,
            TestAnswersValidator answersValidator)
        {
            _answersManager = answersManager;
            _answersValidator = answersValidator;
            _questionsManager = questionsManager;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> CreateTestAnswer([FromBody] TestAnswerPayload answerPayload)
        {
            var validationResult = await _answersValidator.Validate(answerPayload);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            var createdAnswer = await _answersManager.CreateTestAnswer(answerPayload);
            return CreatedAtAction(nameof(GetTestAnswer), new { id = createdAnswer.Id }, createdAnswer);
        }

        [HttpGet]
        [Route("{answerId}")]
        [Authorize]
        public async Task<IActionResult> GetTestAnswer(int questionId, int answerId)
        {
            if (!await _answersManager.Exists(answerId, questionId))
                return NotFound();

            var answer = await _answersManager.GetTestAnswer(answerId);
            return Ok(answer);
        }

        [HttpGet]
        [Route("")]
        [Authorize]
        public async Task<IActionResult> GetTestAnswers(int testId, int questionId)
        {
            if (!await _questionsManager.Exists(questionId, testId))
                return NotFound();

            var answers = await _answersManager.GetTestAnswers(questionId);
            return Ok(answers);
        }

        [HttpDelete]
        [Route("{answerId}")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> DeleteTestAnswer(int questionId, int answerId)
        {
            if (!await _answersManager.Exists(answerId, questionId))
                return NotFound();

            await _answersManager.DeleteTestAnswer(answerId);
            return StatusCode((int)HttpStatusCode.NoContent);
        }

        [HttpPut]
        [Route("{answerId}")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> UpdateTestAnswer(int questionId, int answerId, [FromBody] TestAnswerPayload answerPayload)
        {
            if (!await _answersManager.Exists(answerId, questionId))
                return NotFound();

            var validationResult = await _answersValidator.Validate(answerPayload);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            var updatedAnswer = await _answersManager.UpdateTestAnswer(answerId, answerPayload);
            return Ok(updatedAnswer);
        }
    }
}
