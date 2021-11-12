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
    public class AccountController : Controller
    {
        private readonly AccountManager _accountManager;
        private readonly AccountValidator _accountValidator;
        private readonly IUserContextManager _userContextManager;

        public AccountController(AccountManager accountManager, AccountValidator accountValidator, IUserContextManager userContextManager)
        {
            _accountManager = accountManager;
            _accountValidator = accountValidator;
            _userContextManager = userContextManager;
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
            var validationResult = await _accountValidator.ValidateOnLogin(loginModel);

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
            var validationResult = await _accountValidator.ValidateOnRegister(registerModel);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            var authModel = await _accountManager.Register(registerModel);

            if (string.IsNullOrEmpty(authModel.AccessToken))
                return StatusCode(500);

            return Ok(authModel);
        }

        [HttpPost]
        [Route("confirm-email")]
        [Authorize]
        public async Task<IActionResult> ConfirmEmail(string code)
        {
            var emailConfirmed = await _accountManager.ConfirmEmail(code);

            if (!emailConfirmed)
                return StatusCode(500);

            return Ok();
        }

        [HttpPost]
        [Route("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordPayload changePasswordPayload)
        {
            var userId = _userContextManager.CurrentUser.Id;
            var validationResult = await _accountValidator.ValidateOnChangePassword(userId, changePasswordPayload);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            var passwordChanged = await _accountManager.ChangePassword(changePasswordPayload);

            if (!passwordChanged)
                return StatusCode(500);

            return Ok();
        }

        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPasswordRequest([FromBody] ResetPasswordPayload resetPasswordPayload)
        {
            var validationResult = await _accountValidator.ValidateOnPasswordResetRequest(resetPasswordPayload);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            await _accountManager.ResetPasswordRequest(resetPasswordPayload.Email);
            return Ok();
        }

        [HttpPut]
        [Route("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordPayload resetPasswordPayload)
        {
            var validationResult = _accountValidator.ValidateOnPasswordReset(resetPasswordPayload);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            var passwordChanged = await _accountManager.ResetPassword(resetPasswordPayload.UserId, resetPasswordPayload);

            if (!passwordChanged)
                return StatusCode(500);

            return Ok();
        }
    }
}
