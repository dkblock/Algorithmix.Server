using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using VisualAlgorithms.Common.Constants;
using VisualAlgorithms.Mappers;
using VisualAlgorithms.Models.Tests;
using VisualAlgorithms.Server.Validation;
using VisualAlgorithms.Services;

namespace VisualAlgorithms.Server.Controllers
{
    [ApiController]
    [Route("api/tests")]
    public class TestsController : Controller
    {
        private readonly TestsMapper _testsMapper;
        private readonly TestsService _testsService;
        private readonly TestsValidator _testsValidator;

        public TestsController(TestsMapper testsMapper, TestsService testsService, TestsValidator testsValidator)
        {
            _testsMapper = testsMapper;
            _testsService = testsService;
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

            var createdTest = await _testsService.CreateTest(testPayload);
            return CreatedAtAction(nameof(GetTest), new { id = createdTest.Id }, createdTest);
        }

        [HttpGet]
        [Route("{testId}")]
        [Authorize]
        public async Task<IActionResult> GetTest(int testId)
        {
            if (!await _testsService.Exists(testId))
                return NotFound();

            var test = await _testsService.GetTest(testId);
            return Ok(test);
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetTests()
        {
            var tests = await _testsService.GetTests();
            return Ok(tests);
        }

        [HttpDelete]
        [Route("{testId}")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> DeleteTest(int testId)
        {
            if (!await _testsService.Exists(testId))
                return NotFound();

            await _testsService.DeleteTest(testId);
            return StatusCode((int)HttpStatusCode.NoContent);
        }

        [HttpPut]
        [Route("{testId}")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> UpdateTest(int testId, TestPayload testPayload)
        {
            if (!await _testsService.Exists(testId))
                return NotFound();

            var validationResult = await _testsValidator.Validate(testPayload);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            var updatedTest = await _testsService.UpdateTest(testId, testPayload);
            return Ok(updatedTest);
        }
    }
}
