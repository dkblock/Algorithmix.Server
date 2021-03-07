using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VisualAlgorithms.Models.Account;
using VisualAlgorithms.Server.Validation;
using VisualAlgorithms.Services;

namespace VisualAlgorithms.Server.Controllers
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

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var validationResult = await _accountValidator.Validate(loginModel);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            var token = await _accountService.Login(loginModel);

            if (string.IsNullOrEmpty(token))
                return StatusCode(500);

            return Ok(token);
        }

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            var validationResult = await _accountValidator.Validate(registerModel);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            var token = await _accountService.Register(registerModel);

            if (string.IsNullOrEmpty(token))
                return StatusCode(500);

            return Ok(token);
        }
    }
}
