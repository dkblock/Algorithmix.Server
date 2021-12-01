using Algorithmix.Api.Core.TestDesign;
using Algorithmix.Api.Core.TestPass;
using Algorithmix.Api.Validation;
using Algorithmix.Common.Constants;
using Algorithmix.Models.Tests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Algorithmix.Server.Controllers
{
    [ApiController]
    [Route("api/tests")]
    public class TestController : Controller
    {
        private readonly PublishedTestManager _pubTestManager;
        private readonly TestManager _testManager;
        private readonly TestValidator _testValidator;

        public TestController(
            PublishedTestManager pubTestManager,
            TestManager testManager,
            TestValidator testValidator)
        {
            _pubTestManager = pubTestManager;
            _testManager = testManager;
            _testValidator = testValidator;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> CreateTest([FromBody] TestPayload testPayload)
        {
            var validationResult = await _testValidator.Validate(testPayload);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            var createdTest = await _testManager.CreateTest(testPayload);
            return CreatedAtAction(nameof(GetTest), new { testId = createdTest.Id }, createdTest);
        }

        [HttpGet]
        [Route("{testId}")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> GetTest(int testId)
        {
            if (!await _testManager.Exists(testId))
                return NotFound();

            var test = await _testManager.GetTest(testId);

            if (!test.UserHasAccess)
                return Forbid();

            return Ok(test);
        }

        [HttpGet]
        [Route("published/{testId}")]
        [Authorize]
        public async Task<IActionResult> GetPublishedTest(int testId)
        {
            if (!await _pubTestManager.Exists(testId))
                return NotFound();

            var test = await _pubTestManager.GetTest(testId);
            return Ok(test);
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetTests(
            string searchText = "",
            bool onlyAccessible = false,
            int pageIndex = 1,
            int pageSize = 20,
            TestSortBy sortBy = TestSortBy.CreatedDate,
            bool desc = true)
        {
            var query = new TestQuery(searchText, onlyAccessible, pageSize, pageIndex, sortBy, desc);
            var tests = await _testManager.GetTests(query);

            return Ok(tests);
        }

        [HttpGet]
        [Route("published")]
        public async Task<IActionResult> GetPublishedTests(
            string searchText = "",
            int pageIndex = 1,
            int pageSize = 20,
            TestSortBy sortBy = TestSortBy.CreatedDate,
            bool desc = true)
        {
            var query = new TestQuery(searchText, false, pageSize, pageIndex, sortBy, desc);
            var testResponse = await _pubTestManager.GetTests(query);

            return Ok(testResponse);
        }

        [HttpDelete]
        [Route("{testId}")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> DeleteTest(int testId)
        {
            if (!await _testManager.Exists(testId))
                return NotFound();

            var test = await _testManager.GetTest(testId);

            if (!test.UserHasAccess)
                return Forbid();

            if (await _pubTestManager.Exists(testId))
                await _pubTestManager.DeleteTest(testId);

            await _testManager.DeleteTest(testId);

            return NoContent();
        }

        [HttpPut]
        [Route("{testId}")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> UpdateTest(int testId, TestPayload testPayload)
        {
            if (!await _testManager.Exists(testId))
                return NotFound();

            var test = await _testManager.GetTest(testId);

            if (!test.UserHasAccess)
                return Forbid();

            var validationResult = await _testValidator.Validate(testPayload);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            var updatedTest = await _testManager.UpdateTest(testId, testPayload);
            return Ok(updatedTest);
        }
    }
}
