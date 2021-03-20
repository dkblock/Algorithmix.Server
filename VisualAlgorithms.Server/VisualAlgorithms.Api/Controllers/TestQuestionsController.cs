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
    [Route("api/tests/{testId}/questions")]
    [Authorize]
    public class TestQuestionsController : Controller
    {
        private readonly TestQuestionsManager _questionsManager;
        private readonly TestsManager _testsManager;
        private readonly TestQuestionsValidator _questionsValidator;

        public TestQuestionsController(TestQuestionsManager questionsManager, TestsManager testsManager, TestQuestionsValidator questionsValidator)
        {
            _questionsManager = questionsManager;
            _questionsValidator = questionsValidator;
            _testsManager = testsManager;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> CreateTestQuestion([FromBody] TestQuestionPayload questionPayload)
        {
            var validationResult = await _questionsValidator.Validate(questionPayload);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            var createdQuestion = await _questionsManager.CreateTestQuestion(questionPayload);
            return CreatedAtAction(nameof(GetTestQuestion), new { id = createdQuestion.Id }, createdQuestion);
        }

        [HttpGet]
        [Route("{questionId}")]
        [Authorize]
        public async Task<IActionResult> GetTestQuestion(int testId, int questionId)
        {
            if (!await _questionsManager.Exists(questionId, testId))
                return NotFound();

            var question = await _questionsManager.GetTestQuestion(questionId);
            return Ok(question);
        }

        [HttpGet]
        [Route("")]
        [Authorize]
        public async Task<IActionResult> GetTestQuestions(int testId)
        {
            if (!await _testsManager.Exists(testId))
                return NotFound();

            var questions = await _questionsManager.GetTestQuestions(testId);
            return Ok(questions);
        }

        [HttpDelete]
        [Route("{questionId}")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> DeleteTestQuestion(int testId, int questionId)
        {
            if (!await _questionsManager.Exists(questionId, testId))
                return NotFound();

            await _questionsManager.DeleteTestQuestion(questionId);
            return StatusCode((int)HttpStatusCode.NoContent);
        }

        [HttpPut]
        [Route("{questionId}")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> UpdateTestQuestion(int testId, int questionId, [FromBody] TestQuestionPayload questionPayload)
        {
            if (!await _questionsManager.Exists(questionId, testId))
                return NotFound();

            var validationResult = await _questionsValidator.Validate(questionPayload);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            var updatedQuestion = await _questionsManager.UpdateTestQuestion(questionId, questionPayload);
            return Ok(updatedQuestion);
        }
    }
}
