using Algorithmix.Api.Core;
using Algorithmix.Api.Validation;
using Algorithmix.Common.Constants;
using Algorithmix.Models.Algorithms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Algorithmix.Server.Controllers
{
    [ApiController]
    [Route("api/algorithms")]
    public class AlgorithmController : Controller
    {
        private readonly AlgorithmManager _algorithmManager;
        private readonly AlgorithmValidator _algorithmValidator;

        public AlgorithmController(AlgorithmManager algorithmManager, AlgorithmValidator algorithmValidator)
        {
            _algorithmManager = algorithmManager;
            _algorithmValidator = algorithmValidator;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> CreateAlgorithm([FromBody] AlgorithmPayload algorithmPayload)
        {
            var validationResult = await _algorithmValidator.ValidateAlgorithm(algorithmPayload, true);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            var createdAlgorithm = await _algorithmManager.CreateAlgorithm(algorithmPayload);
            return CreatedAtAction(nameof(GetAlgorithm), new { algorithmId = createdAlgorithm.Id }, createdAlgorithm);
        }

        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAlgorithms(
            string searchText = "",
            bool onlyAccessible = false,
            int pageIndex = 1,
            int pageSize = 100,
            AlgorithmSortBy sortBy = AlgorithmSortBy.None,
            bool desc = false)
        {
            var query = new AlgorithmQuery(searchText, onlyAccessible, pageIndex, pageSize, sortBy, desc);
            var algorithms = await _algorithmManager.GetAlgorithms(query);

            return Ok(algorithms);
        }

        [HttpGet]
        [Route("{algorithmId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAlgorithm(string algorithmId)
        {
            if (!await _algorithmManager.Exists(algorithmId))
                return NotFound();

            if (!await UserHasAccess(algorithmId))
                return Forbid();

            var algorithm = await _algorithmManager.GetAlgorithm(algorithmId);
            return Ok(algorithm);
        }

        [HttpDelete]
        [Route("{algorithmId}")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> DeleteAlgorithm(string algorithmId)
        {
            if (!await _algorithmManager.Exists(algorithmId))
                return NotFound();

            if (!await UserHasAccess(algorithmId))
                return Forbid();

            await _algorithmManager.DeleteAlgorithm(algorithmId);
            return NoContent();
        }

        [HttpPut]
        [Route("{algorithmId}")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> UpdateAlgorithm(string algorithmId, [FromBody] AlgorithmPayload algorithmPayload)
        {
            if (!await _algorithmManager.Exists(algorithmId))
                return NotFound();

            if (!await UserHasAccess(algorithmId))
                return Forbid();

            var validationResult = await _algorithmValidator.ValidateAlgorithm(algorithmPayload, false);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            var updatedAlgorithm = await _algorithmManager.UpdateAlgorithm(algorithmId, algorithmPayload);
            return Ok(updatedAlgorithm);
        }

        [HttpPut]
        [Route("{algorithmId}/time-complexity")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> UpdateAlgorithmTimeComplexity(string algorithmId, [FromBody] AlgorithmTimeComplexityPayload timeComplexityPayload)
        {
            if (!await _algorithmManager.Exists(algorithmId))
                return NotFound();

            if (!await UserHasAccess(algorithmId))
                return Forbid();

            var updatedAlgorithmTimeComplexity = await _algorithmManager.UpdateAlgorithmTimeComplexity(timeComplexityPayload.Id, timeComplexityPayload);
            return Ok(updatedAlgorithmTimeComplexity);
        }

        [HttpPost]
        [Route("{algorithmId}/description")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> UploadAlgorithmDescription(string algorithmId)
        {
            if (!await _algorithmManager.Exists(algorithmId))
                return NotFound();

            if (!await UserHasAccess(algorithmId))
                return Forbid();

            var file = HttpContext.Request.Form.Files.FirstOrDefault();

            if (file == null)
                return NoContent();

            var validationResult = _algorithmValidator.ValidateAlgorithmData(algorithmId, file);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            _algorithmManager.UpdateAlgorithmDescription(algorithmId, file);
            return Ok();
        }

        [HttpDelete]
        [Route("{algorithmId}/description")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> ClearAlgorithmDescription(string algorithmId)
        {
            if (!await _algorithmManager.Exists(algorithmId))
                return NotFound();

            if (!await UserHasAccess(algorithmId))
                return Forbid();

            _algorithmManager.DeleteAlgorithmDescription(algorithmId);
            return NoContent();
        }

        [HttpGet]
        [Route("{algorithmId}/description")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> DownloadAlgorithmDescription(string algorithmId)
        {
            if (!await _algorithmManager.Exists(algorithmId))
                return NotFound();

            if (!await UserHasAccess(algorithmId))
                return Forbid();

            var description = _algorithmManager.DownloadAlgorithmDescription(algorithmId);
            return Ok(description);
        }

        [HttpPost]
        [Route("{algorithmId}/constructor")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> UploadAlgorithmConstructor(string algorithmId)
        {
            if (!await _algorithmManager.Exists(algorithmId))
                return NotFound();

            if (!await UserHasAccess(algorithmId))
                return Forbid();

            var file = HttpContext.Request.Form.Files.FirstOrDefault();

            if (file == null)
                return NoContent();

            var validationResult = _algorithmValidator.ValidateAlgorithmData(algorithmId, file);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            _algorithmManager.UpdateAlgorithmConstructor(algorithmId, file);
            return Ok();
        }

        [HttpDelete]
        [Route("{algorithmId}/constructor")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> ClearAlgorithmConstructor(string algorithmId)
        {
            if (!await _algorithmManager.Exists(algorithmId))
                return NotFound();

            if (!await UserHasAccess(algorithmId))
                return Forbid();

            _algorithmManager.DeleteAlgorithmConstructor(algorithmId);
            return NoContent();
        }

        [HttpGet]
        [Route("{algorithmId}/constructor")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> DownloadAlgorithmConstructor(string algorithmId)
        {
            if (!await _algorithmManager.Exists(algorithmId))
                return NotFound();

            if (!await UserHasAccess(algorithmId))
                return Forbid();

            var constructor = _algorithmManager.DownloadAlgorithmConstructor(algorithmId);
            return Ok(constructor);
        }

        [HttpPost]
        [Route("{algorithmId}/image")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> UploadAlgorithmImage(string algorithmId)
        {
            if (!await _algorithmManager.Exists(algorithmId))
                return NotFound();

            if (!await UserHasAccess(algorithmId))
                return Forbid();

            var image = HttpContext.Request.Form.Files.FirstOrDefault();

            if (image == null)
                return NoContent();

            var updatedAlgorithm = await _algorithmManager.UpdateAlgorithmImage(algorithmId, image);
            return Ok(updatedAlgorithm);
        }

        [HttpDelete]
        [Route("{algorithmId}/image")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> ClearAlgorithmImage(string algorithmId)
        {
            if (!await _algorithmManager.Exists(algorithmId))
                return NotFound();

            if (!await UserHasAccess(algorithmId))
                return Forbid();

            await _algorithmManager.ClearAlgorithmImage(algorithmId);
            return NoContent();
        }

        [HttpGet]
        [Route("{algorithmId}/data/template")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> DownloadAlgorithmDataTemplate(string algorithmId)
        {
            if (!await _algorithmManager.Exists(algorithmId))
                return NotFound();

            if (!await UserHasAccess(algorithmId))
                return Forbid();

            var dataTemplate = _algorithmManager.GetAlgorithmDataTemplate(algorithmId);
            return Ok(dataTemplate);
        }

        private async Task<bool> UserHasAccess(string algorithmId)
        {
            var algorithm = await _algorithmManager.GetAlgorithm(algorithmId);
            return algorithm.UserHasAccess;
        }
    }
}
