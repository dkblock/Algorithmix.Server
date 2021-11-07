using Algorithmix.Api.Core;
using Algorithmix.Api.Validation;
using Algorithmix.Common.Constants;
using Algorithmix.Models.Algorithms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Algorithmix.Server.Controllers
{
    [ApiController]
    [Route("api/algorithms")]
    [Authorize]
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
        [Authorize(Roles = Roles.Administrator)]
        public async Task<IActionResult> CreateAlgorithm([FromBody] AlgorithmPayload algorithmPayload)
        {
            var validationResult = await _algorithmValidator.Validate(algorithmPayload, true);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            var createdAlgorithm = await _algorithmManager.CreateAlgorithm(algorithmPayload);
            return CreatedAtAction(nameof(GetAlgorithm), new { algorithmId = createdAlgorithm.Id }, createdAlgorithm);
        }

        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAlgorithms(string searchText = "")
        {
            var query = new AlgorithmQuery(searchText);
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

            var algorithm = await _algorithmManager.GetAlgorithm(algorithmId);
            return Ok(algorithm);
        }

        [HttpDelete]
        [Route("{algorithmId}")]
        [Authorize(Roles = Roles.Administrator)]
        public async Task<IActionResult> DeleteAlgorithm(string algorithmId)
        {
            if (!await _algorithmManager.Exists(algorithmId))
                return NotFound();

            await _algorithmManager.DeleteAlgorithm(algorithmId);
            return NoContent();
        }

        [HttpPut]
        [Route("{algorithmId}")]
        [Authorize(Roles = Roles.Administrator)]
        public async Task<IActionResult> UpdateAlgorithm(string algorithmId, [FromBody] AlgorithmPayload algorithmPayload)
        {
            if (!await _algorithmManager.Exists(algorithmId))
                return NotFound();

            var validationResult = await _algorithmValidator.Validate(algorithmPayload, false);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            var updatedAlgorithm = await _algorithmManager.UpdateAlgorithm(algorithmId, algorithmPayload);
            return Ok(updatedAlgorithm);
        }

        [HttpPost]
        [Route("{algorithmId}/image")]
        [Authorize(Roles = Roles.Administrator)]
        public async Task<IActionResult> UploadAlgorithmImage(string algorithmId)
        {
            var image = HttpContext.Request.Form.Files.FirstOrDefault();

            if (image == null)
                return NoContent();

            var updatedAlgorithm = await _algorithmManager.UpdateAlgorithmImage(algorithmId, image);
            return Ok(updatedAlgorithm);
        }

        [HttpDelete]
        [Route("{algorithmId}/image")]
        [Authorize(Roles = Roles.Administrator)]
        public async Task<IActionResult> ClearAlgorithmImage(string algorithmId)
        {
            await _algorithmManager.ClearAlgorithmImage(algorithmId);
            return NoContent();
        }
    }
}
