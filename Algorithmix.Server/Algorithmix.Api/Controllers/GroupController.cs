using Algorithmix.Api.Core;
using Algorithmix.Api.Validation;
using Algorithmix.Common.Constants;
using Algorithmix.Models.Groups;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Algorithmix.Api.Controllers
{
    [ApiController]
    [Route("api/groups")]
    public class GroupController : Controller
    {
        private readonly GroupManager _groupManager;
        private readonly GroupValidator _groupValidator;

        public GroupController(GroupManager groupManager, GroupValidator groupValidator)
        {
            _groupManager = groupManager;
            _groupValidator = groupValidator;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = Roles.Administrator)]
        public async Task<IActionResult> CreateGroup(GroupPayload groupPayload)
        {
            var validationResult = await _groupValidator.Validate(groupPayload);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            var createdGroup = await _groupManager.CreateGroup(groupPayload);
            return CreatedAtAction(nameof(GetGroup), new { groupId = createdGroup.Id }, createdGroup);
        }

        [HttpGet]
        [Route("{groupId}")]
        public async Task<IActionResult> GetGroup(int groupId)
        {
            if (!await _groupManager.Exists(groupId))
                return NotFound();

            var group = await _groupManager.GetGroup(groupId);
            return Ok(group);
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllGroups()
        {
            var groups = await _groupManager.GetAllGroups();
            return Ok(groups);
        }

        [HttpDelete]
        [Route("{groupId}")]
        [Authorize(Roles = Roles.Administrator)]
        public async Task<IActionResult> DeleteGroup(int groupId)
        {
            if (!await _groupManager.Exists(groupId))
                return NotFound();

            await _groupManager.DeleteGroup(groupId);
            return NoContent();
        }

        [HttpPut]
        [Route("{groupId}")]
        [Authorize(Roles = Roles.Administrator)]
        public async Task<IActionResult> UpdateGroup(int groupId, GroupPayload groupPayload)
        {
            if (!await _groupManager.Exists(groupId))
                return NotFound();

            var validationResult = await _groupValidator.Validate(groupPayload, groupId);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            var updatedGroup = await _groupManager.UpdateGroup(groupId, groupPayload);
            return Ok(updatedGroup);
        }
    }
}