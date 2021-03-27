using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VisualAlgorithms.Common.Constants;
using VisualAlgorithms.Database;
using VisualAlgorithms.Services;

namespace VisualAlgorithms.Server.Controllers
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
