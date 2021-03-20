using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using VisualAlgorithms.Api.Managers;
using VisualAlgorithms.Api.Validation;
using VisualAlgorithms.Common.Constants;
using VisualAlgorithms.Models.Tests;

namespace VisualAlgorithms.Server.Controllers
{
    [ApiController]
    [Route("api/tests")]
    public class TestsController : Controller
    {
        private readonly TestsManager _testsManager;
        private readonly TestsValidator _testsValidator;

        public TestsController(TestsManager testsManager, TestsValidator testsValidator)
        {
            _testsManager = testsManager;
            _testsValidator = testsValidator;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> CreateTest([FromBody] TestPayload testPayload)
        {
            var validationResult = await _testsValidator.Validate(testPayload);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            var createdTest = await _testsManager.CreateTest(testPayload);
            return CreatedAtAction(nameof(GetTest), new { id = createdTest.Id }, createdTest);
        }

        [HttpGet]
        [Route("{testId}")]
        [Authorize]
        public async Task<IActionResult> GetTest(int testId)
        {
            if (!await _testsManager.Exists(testId))
                return NotFound();

            var test = await _testsManager.GetTest(testId);
            return Ok(test);
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetTests()
        {
            var tests = await _testsManager.GetTests();
            return Ok(tests);
        }

        [HttpDelete]
        [Route("{testId}")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> DeleteTest(int testId)
        {
            if (!await _testsManager.Exists(testId))
                return NotFound();

            await _testsManager.DeleteTest(testId);
            return StatusCode((int)HttpStatusCode.NoContent);
        }

        [HttpPut]
        [Route("{testId}")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> UpdateTest(int testId, TestPayload testPayload)
        {
            if (!await _testsManager.Exists(testId))
                return NotFound();

            var validationResult = await _testsValidator.Validate(testPayload);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            var updatedTest = await _testsManager.UpdateTest(testId, testPayload);
            return Ok(updatedTest);
        }
    }
}
