using Algorithmix.Api.Core;
using Algorithmix.Api.Core.TestDesign;
using Algorithmix.Api.Validation;
using Algorithmix.Common.Constants;
using Algorithmix.Models;
using Algorithmix.Models.Tests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Algorithmix.Api.Controllers
{
    [ApiController]
    [Route("api/tests/{testId}/questions")]
    [Authorize]
    public class TestQuestionController : Controller
    {
        private readonly TestQuestionManager _questionManager;
        private readonly TestManager _testManager;
        private readonly TestDataManager _testDataManager;
        private readonly TestQuestionValidator _questionValidator;

        public TestQuestionController(
            TestQuestionManager questionManager,
            TestManager testManager,
            TestDataManager testDataManager,
            TestQuestionValidator questionValidator)
        {
            _questionManager = questionManager;
            _testManager = testManager;
            _testDataManager = testDataManager;
            _questionValidator = questionValidator;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> CreateTestQuestion(int testId, [FromBody] TestQuestionPayload questionPayload)
        {
            var validationResult = await _questionValidator.Validate(questionPayload);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            var createdQuestion = await _questionManager.CreateTestQuestion(questionPayload);
            return CreatedAtAction(nameof(GetTestQuestion), new { testId, questionId = createdQuestion.Id }, createdQuestion);
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

        [HttpPut]
        [Route("move")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> MoveTestQuestions(int testId, [FromBody] MovePayload movePayload)
        {
            var movedQuestions = await _questionManager.MoveTestQuestion(testId, movePayload.OldIndex, movePayload.NewIndex);
            return Ok(movedQuestions);
        }

        [HttpPost]
        [Route("{questionId}/image")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> UploadTestQuestionImage(int testId, int questionId)
        {
            var image = HttpContext.Request.Form.Files.FirstOrDefault();

            if (image == null)
                return NoContent();

            var imagePath = _testDataManager.CreateTestQuestionImage(testId, questionId, image);
            var updatedQuestion = await _questionManager.UpdateTestQuestionImage(questionId, imagePath);

            return Ok(updatedQuestion);
        }

        [HttpDelete]
        [Route("{questionId}/image")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> ClearTestQuestionImage(int questionId)
        {
            var imagepath = await _questionManager.ClearTestQuestionImage(questionId);
            _testDataManager.DeleteTestQuestionImage(imagepath);

            return NoContent();
        }
    }
}
