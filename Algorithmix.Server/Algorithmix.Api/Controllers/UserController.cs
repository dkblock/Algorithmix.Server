using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Algorithmix.Common.Constants;
using Algorithmix.Database;
using Algorithmix.Services;

namespace Algorithmix.Server.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize(Roles = Roles.User)]
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = await _userService.GetUser(User);
            return Ok("test");
        }
    }
}
