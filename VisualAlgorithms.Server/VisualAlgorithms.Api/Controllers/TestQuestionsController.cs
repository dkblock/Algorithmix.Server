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
    [Route("api/tests/{testId}/questions")]
    [Authorize]
    public class TestQuestionsController : Controller
    {
        private readonly TestQuestionsService _questionsService;
        private readonly TestQuestionsValidator _questionsValidator;
        private readonly TestsService _testsService;

        public TestQuestionsController(
            TestQuestionsService questionsService, 
            TestQuestionsValidator questionsValidator,
            TestsService testsService)
        {
            _questionsService = questionsService;
            _questionsValidator = questionsValidator;
            _testsService = testsService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> CreateTestQuestion([FromBody] TestQuestionPayload questionPayload)
        {
            var validationResult = await _questionsValidator.Validate(questionPayload);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            var createdQuestion = await _questionsService.CreateTestQuestion(questionPayload);
            return CreatedAtAction(nameof(GetTestQuestion), new { id = createdQuestion.Id }, createdQuestion);
        }

        [HttpGet]
        [Route("{questionId}")]
        [Authorize]
        public async Task<IActionResult> GetTestQuestion(int testId, int questionId)
        {
            if (!await _questionsService.Exists(questionId, testId))
                return NotFound();

            var question = await _questionsService.GetTestQuestion(questionId);
            return Ok(question);
        }

        [HttpGet]
        [Route("")]
        [Authorize]
        public async Task<IActionResult> GetTestQuestions(int testId)
        {
            if (!await _testsService.Exists(testId))
                return NotFound();

            var questions = await _questionsService.GetTestQuestions(testId);
            return Ok(questions);
        }

        [HttpDelete]
        [Route("{questionId}")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> DeleteTestQuestion(int testId, int questionId)
        {
            if (!await _questionsService.Exists(questionId, testId))
                return NotFound();

            await _questionsService.DeleteTestQuestion(questionId);
            return StatusCode((int)HttpStatusCode.NoContent);
        }

        [HttpPut]
        [Route("{questionId}")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> UpdateTestQuestion(int testId, int questionId, [FromBody] TestQuestionPayload questionPayload)
        {
            if (!await _questionsService.Exists(questionId, testId))
                return NotFound();

            var validationResult = await _questionsValidator.Validate(questionPayload);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            var updatedQuestion = await _questionsService.UpdateTestQuestion(questionId, questionPayload);
            return Ok(updatedQuestion);
        }
    }
}
