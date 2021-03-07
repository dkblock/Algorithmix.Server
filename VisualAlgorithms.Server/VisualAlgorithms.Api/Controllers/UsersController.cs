using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisualAlgorithms.Common.Constants;
using VisualAlgorithms.Database;

namespace VisualAlgorithms.Server.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize(Roles = Roles.Executive)]
    public class UsersController : Controller
    {
        private readonly ApplicationContext _db;

        public UsersController(ApplicationContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("test");
        }
    }
}
