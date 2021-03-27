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
    public class TestAnswerController : Controller
    {
        private readonly TestAnswerManager _answerManager;
        private readonly TestQuestionManager _questionManager;
        private readonly TestAnswerValidator _answerValidator;

        public TestAnswerController(
            TestAnswerManager answerManager,
            TestQuestionManager questionManager,
            TestAnswerValidator answerValidator)
        {
            _answerManager = answerManager;
            _answerValidator = answerValidator;
            _questionManager = questionManager;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> CreateTestAnswer([FromBody] TestAnswerPayload answerPayload)
        {
            var validationResult = await _answerValidator.Validate(answerPayload);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            var createdAnswer = await _answerManager.CreateTestAnswer(answerPayload);
            return CreatedAtAction(nameof(GetTestAnswer), new { id = createdAnswer.Id }, createdAnswer);
        }

        [HttpGet]
        [Route("{answerId}")]
        [Authorize]
        public async Task<IActionResult> GetTestAnswer(int questionId, int answerId)
        {
            if (!await _answerManager.Exists(answerId, questionId))
                return NotFound();

            var answer = await _answerManager.GetTestAnswer(answerId);
            return Ok(answer);
        }

        [HttpGet]
        [Route("")]
        [Authorize]
        public async Task<IActionResult> GetTestAnswers(int testId, int questionId)
        {
            if (!await _questionManager.Exists(questionId, testId))
                return NotFound();

            var answers = await _answerManager.GetTestAnswers(questionId);
            return Ok(answers);
        }

        [HttpDelete]
        [Route("{answerId}")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> DeleteTestAnswer(int questionId, int answerId)
        {
            if (!await _answerManager.Exists(answerId, questionId))
                return NotFound();

            await _answerManager.DeleteTestAnswer(answerId);
            return StatusCode((int)HttpStatusCode.NoContent);
        }

        [HttpPut]
        [Route("{answerId}")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> UpdateTestAnswer(int questionId, int answerId, [FromBody] TestAnswerPayload answerPayload)
        {
            if (!await _answerManager.Exists(answerId, questionId))
                return NotFound();

            var validationResult = await _answerValidator.Validate(answerPayload);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            var updatedAnswer = await _answerManager.UpdateTestAnswer(answerId, answerPayload);
            return Ok(updatedAnswer);
        }
    }
}
