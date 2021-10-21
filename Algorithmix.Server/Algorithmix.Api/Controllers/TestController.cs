﻿using Algorithmix.Api.Core.TestDesign;
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
        [Authorize]
        public async Task<IActionResult> GetTest(int testId)
        {
            if (!await _testManager.Exists(testId))
                return NotFound();

            var test = await _testManager.GetTest(testId);

            return Ok(test);
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetTests(string searchText = "")
        {
            var query = new TestQuery(searchText);
            var tests = await _testManager.GetTests(query);

            return Ok(tests);
        }

        [HttpGet]
        [Route("published")]
        public async Task<IActionResult> GetPublishedTests(string searchText = "")
        {
            var query = new TestQuery(searchText);
            var tests = await _pubTestManager.GetTests(query);

            return Ok(tests);
        }

        [HttpDelete]
        [Route("{testId}")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> DeleteTest(int testId)
        {
            if (!await _testManager.Exists(testId))
                return NotFound();

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

            var validationResult = await _testValidator.Validate(testPayload);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            var updatedTest = await _testManager.UpdateTest(testId, testPayload);
            return Ok(updatedTest);
        }
    }
}
