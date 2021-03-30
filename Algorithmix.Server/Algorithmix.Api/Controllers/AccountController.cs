using Algorithmix.Api.Validation;
using Algorithmix.Models.Account;
using Algorithmix.Services;
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
        private readonly AccountService _accountService;
        private readonly AccountValidator _accountValidator;

        public AccountController(AccountService accountService, AccountValidator accountValidator)
        {
            _accountService = accountService;
            _accountValidator = accountValidator;
        }

        [HttpGet]
        [Route("auth")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromHeader] string authorization)
        {
            var authModel = await _accountService.Authenticate(authorization);
            return Ok(authModel);
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var validationResult = await _accountValidator.Validate(loginModel);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            var authModel = await _accountService.Login(loginModel);

            if (string.IsNullOrEmpty(authModel.AccessToken))
                return StatusCode(500);

            return Ok(authModel);
        }

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            var validationResult = await _accountValidator.Validate(registerModel);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            var authModel = await _accountService.Register(registerModel);

            if (string.IsNullOrEmpty(authModel.AccessToken))
                return StatusCode(500);

            return Ok(authModel);
        }
    }
}
