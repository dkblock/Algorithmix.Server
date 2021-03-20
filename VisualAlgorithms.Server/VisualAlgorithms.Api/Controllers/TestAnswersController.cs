using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using VisualAlgorithms.Common.Constants;
using VisualAlgorithms.Models.Tests;
using VisualAlgorithms.Server.Validation;
using VisualAlgorithms.Services;

namespace VisualAlgorithms.Api.Controllers
{
    [ApiController]
    [Route("api/tests/{testId}/questions/{questionId}/answers")]
    [Authorize]
    public class TestAnswersController : Controller
    {
        private readonly TestAnswersService _answersService;
        private readonly TestAnswersValidator _answersValidator;
        private readonly TestQuestionsService _questionsService;

        public TestAnswersController(
            TestAnswersService answersService,
            TestAnswersValidator answersValidator,
            TestQuestionsService questionsService)
        {
            _answersService = answersService;
            _answersValidator = answersValidator;
            _questionsService = questionsService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> CreateTestAnswer([FromBody] TestAnswerPayload answerPayload)
        {
            var validationResult = await _answersValidator.Validate(answerPayload);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            var createdAnswer = await _answersService.CreateTestAnswer(answerPayload);
            return CreatedAtAction(nameof(GetTestAnswer), new { id = createdAnswer.Id }, createdAnswer);
        }

        [HttpGet]
        [Route("{answerId}")]
        [Authorize]
        public async Task<IActionResult> GetTestAnswer(int questionId, int answerId)
        {
            if (!await _answersService.Exists(answerId, questionId))
                return NotFound();

            var answer = await _answersService.GetTestAnswer(answerId);
            return Ok(answer);
        }

        [HttpGet]
        [Route("")]
        [Authorize]
        public async Task<IActionResult> GetTestAnswers(int testId, int questionId)
        {
            if (!await _questionsService.Exists(questionId, testId))
                return NotFound();

            var answers = await _answersService.GetTestAnswers(questionId);
            return Ok(answers);
        }

        [HttpDelete]
        [Route("{answerId}")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> DeleteTestAnswer(int questionId, int answerId)
        {
            if (!await _answersService.Exists(answerId, questionId))
                return NotFound();

            await _answersService.DeleteTestAnswer(answerId);
            return StatusCode((int)HttpStatusCode.NoContent);
        }

        [HttpPut]
        [Route("{answerId}")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> UpdateTestAnswer(int questionId, int answerId, [FromBody] TestAnswerPayload answerPayload)
        {
            if (!await _answersService.Exists(answerId, questionId))
                return NotFound();

            var validationResult = await _answersValidator.Validate(answerPayload);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            var updatedAnswer = await _answersService.UpdateTestAnswer(answerId, answerPayload);
            return Ok(updatedAnswer);
        }
    }
}
