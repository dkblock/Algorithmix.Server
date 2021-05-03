using Algorithmix.Api.Core;
using Algorithmix.Api.Validation;
using Algorithmix.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Algorithmix.Server.Controllers
{
    [ApiController]
    [Route("api/account")]
    [Authorize]
    public class AccountController : Controller
    {
        private readonly AccountManager _accountManager;
        private readonly AccountValidator _accountValidator;

        public AccountController(AccountManager accountManager, AccountValidator accountValidator)
        {
            _accountManager = accountManager;
            _accountValidator = accountValidator;
        }

        [HttpGet]
        [Route("auth")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromHeader] string authorization)
        {
            var authModel = await _accountManager.Authenticate(authorization);
            return Ok(authModel);
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginPayload loginModel)
        {
            var validationResult = await _accountValidator.Validate(loginModel);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            var authModel = await _accountManager.Login(loginModel);

            if (string.IsNullOrEmpty(authModel.AccessToken))
                return StatusCode(500);

            return Ok(authModel);
        }

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterPayload registerModel)
        {
            var validationResult = await _accountValidator.Validate(registerModel);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            var authModel = await _accountManager.Register(registerModel);

            if (string.IsNullOrEmpty(authModel.AccessToken))
                return StatusCode(500);

            return Ok(authModel);
        }
    }
}
