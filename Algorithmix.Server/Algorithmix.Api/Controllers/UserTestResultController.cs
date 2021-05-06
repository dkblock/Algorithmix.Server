using Algorithmix.Api.Core;
using Algorithmix.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Algorithmix.Api.Controllers
{
    [ApiController]
    [Route("api/test-results")]
    [Authorize(Roles = Roles.Executive)]
    public class UserTestResultController : Controller
    {
        private readonly UserTestResultManager _userTestResultManager;

        public UserTestResultController(UserTestResultManager userTestResultManager)
        {
            _userTestResultManager = userTestResultManager;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetUserTestResults()
        {
            var userTestResults = await _userTestResultManager.GetUserTestResults();
            return Ok(userTestResults);
        }

        [HttpGet]
        [Route("{testId}/{userId}")]
        public async Task<IActionResult> GetUserTestResult(int testId, string userId)
        {
            if (!await _userTestResultManager.Exists(testId, userId))
                return NotFound();

            var userTestResult = await _userTestResultManager.GetUserTestResult(testId, userId);
            return Ok(userTestResult);
        }

        [HttpDelete]
        [Route("{testId}/{userId}")]
        public async Task<IActionResult> DeleteUserTestResult(int testId, string userId)
        {
            if (!await _userTestResultManager.Exists(testId, userId))
                return NotFound();

            await _userTestResultManager.DeleteUserTestResult(testId, userId);
            return NoContent();
        }
    }
}
