using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using Algorithmix.Api.Managers;
using Algorithmix.Api.Validation;
using Algorithmix.Common.Constants;
using Algorithmix.Models.Tests;

namespace Algorithmix.Api.Controllers
{
    [ApiController]
    [Route("api/tests/{testId}/questions")]
    [Authorize]
    public class TestQuestionController : Controller
    {
        private readonly TestQuestionManager _questionManager;
        private readonly TestManager _testManager;
        private readonly TestQuestionValidator _questionValidator;

        public TestQuestionController(TestQuestionManager questionManager, TestManager testManager, TestQuestionValidator questionValidator)
        {
            _questionManager = questionManager;
            _questionValidator = questionValidator;
            _testManager = testManager;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> CreateTestQuestion([FromBody] TestQuestionPayload questionPayload)
        {
            var validationResult = await _questionValidator.Validate(questionPayload);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            var createdQuestion = await _questionManager.CreateTestQuestion(questionPayload);
            return CreatedAtAction(nameof(GetTestQuestion), new { id = createdQuestion.Id }, createdQuestion);
        }

        [HttpGet]
        [Route("{questionId}")]
        [Authorize]
        public async Task<IActionResult> GetTestQuestion(int testId, int questionId)
        {
            if (!await _questionManager.Exists(questionId, testId))
                return NotFound();

            var question = await _questionManager.GetTestQuestion(questionId);
            return Ok(question);
        }

        [HttpGet]
        [Route("")]
        [Authorize]
        public async Task<IActionResult> GetTestQuestions(int testId)
        {
            if (!await _testManager.Exists(testId))
                return NotFound();

            var questions = await _questionManager.GetTestQuestions(testId);
            return Ok(questions);
        }

        [HttpDelete]
        [Route("{questionId}")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> DeleteTestQuestion(int testId, int questionId)
        {
            if (!await _questionManager.Exists(questionId, testId))
                return NotFound();

            await _questionManager.DeleteTestQuestion(questionId);
            return StatusCode((int)HttpStatusCode.NoContent);
        }

        [HttpPut]
        [Route("{questionId}")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> UpdateTestQuestion(int testId, int questionId, [FromBody] TestQuestionPayload questionPayload)
        {
            if (!await _questionManager.Exists(questionId, testId))
                return NotFound();

            var validationResult = await _questionValidator.Validate(questionPayload);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            var updatedQuestion = await _questionManager.UpdateTestQuestion(questionId, questionPayload);
            return Ok(updatedQuestion);
        }
    }
}
