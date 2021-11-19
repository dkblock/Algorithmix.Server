using Algorithmix.Api.Core;
using Algorithmix.Common.Constants;
using Algorithmix.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Algorithmix.Server.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class ApplicationUserController : Controller
    {
        private readonly ApplicationUserManager _userManager;

        public ApplicationUserController(ApplicationUserManager userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        [Route("")]
        [Authorize(Roles = Roles.Executive)]
        public async Task<IActionResult> GetUsers(
            string searchText = "",
            int groupId = -1,
            string role = "all",
            int pageIndex = 1,
            int pageSize = 20,
            ApplicationUserSortBy sortBy = ApplicationUserSortBy.GroupId,
            bool desc = true)
        {
            var query = new ApplicationUserQuery(searchText, groupId, role, pageIndex, pageSize, sortBy, desc);
            var users = await _userManager.GetUsers(query);

            return Ok(users);
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = Roles.Administrator)]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (!await _userManager.Exists(id))
                return NotFound();

            await _userManager.DeleteUser(id);
            return NoContent();
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = Roles.Administrator)]
        public async Task<IActionResult> UpdateUser(string id, ApplicationUserPayload userPayload)
        {
            if (!await _userManager.Exists(id))
                return NotFound();

            var updatedUser = await _userManager.UpdateUser(id, userPayload);
            return Ok(updatedUser);
        }
    }
}
