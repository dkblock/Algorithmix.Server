using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VisualAlgorithms.Models.Account;

namespace VisualAlgorithms.Server.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : Controller
    {
        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] RegisterModel registerModel)
        {
            return Ok();
        }
    }
}
