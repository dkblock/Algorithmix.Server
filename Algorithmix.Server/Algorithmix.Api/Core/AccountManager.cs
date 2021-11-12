using Algorithmix.Identity;
using Algorithmix.Mappers;
using Algorithmix.Models.Account;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Web;

namespace Algorithmix.Api.Core
{
    public class AccountManager
    {
        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationUserMapper _userMapper;
        private readonly AuthenticationService _authService;
        private readonly EmailManager _emailManager;
        private readonly IUserContextManager _userContextManager;

        private readonly string _siteUrl;

        public AccountManager(
            ApplicationUserManager userManager,
            ApplicationUserMapper userMapper,
            AuthenticationService authService,
            EmailManager emailManager,
            IUserContextManager userContextManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _userMapper = userMapper;
            _authService = authService;
            _emailManager = emailManager;
            _userContextManager = userContextManager;

            _siteUrl = configuration["SiteURL"];
        }

        public async Task<UserAccount> Authenticate(string authorization)
        {
            var authModel = _authService.CheckAuth(authorization);
            var user = await _userManager.GetUserById(authModel.CurrentUser.Id);

            return _userMapper.ToModel(user, authModel.AccessToken);
        }

        public async Task<UserAccount> Register(RegisterPayload registerPayload)
        {
            var user = await _userManager.CreateUser(registerPayload);
            var accessToken = _authService.Authenticate(user);

            var code = await _userManager.GenerateEmailConfirmationToken(user.Id);
            var callbackUrl = GetEmailConfirmationUrl(user.Id, code);

            await _emailManager.SendEmail(user.Email, "Регистрация в Algorithmix",
                $"Здравствуйте, {user.FirstName} {user.LastName}!<br/>" +
                "Благодарим Вас за регистрацию на сайте Algorithmix.<br/>" +
                $"Чтобы подтвердить свой адрес электронной почты, <a href='{callbackUrl}'>перейдите по этой ссылке</a>.");

            return _userMapper.ToModel(user, accessToken);
        }

        public async Task<UserAccount> Login(LoginPayload loginPayload)
        {
            var user = await _userManager.GetUserByEmail(loginPayload.Email);
            var accessToken = _authService.Authenticate(user);

            return _userMapper.ToModel(user, accessToken);
        }

        public async Task<bool> ConfirmEmail(string code)
        {
            var userId = _userContextManager.CurrentUser.Id;
            return await _userManager.ConfirmEmail(userId, code);
        }

        public async Task<bool> ChangePassword(ChangePasswordPayload changePasswordPayload)
        {
            var userId = _userContextManager.CurrentUser.Id;
            return await _userManager.ChangePassword(userId, changePasswordPayload);
        }

        public async Task ResetPasswordRequest(string email)
        {
            var user = await _userManager.GetUserByEmail(email);
            var code = await _userManager.GeneratePasswordResetToken(user.Id);
            var callbackUrl = GetPasswordResetUrl(user.Id, HttpUtility.UrlEncode(code));

            await _emailManager.SendEmail(email, "Сброс пароля",
                $"Чтобы сбросить пароль, <a href='{callbackUrl}'>перейдите по этой ссылке</a>.");
        }

        public async Task<bool> ResetPassword(string userId, ResetPasswordPayload resetPasswordPayload)
        {
            return await _userManager.ResetPassword(userId, resetPasswordPayload);
        }

        private string GetEmailConfirmationUrl(string userId, string code)
        {
            return $"{_siteUrl}/account/confirm-email?userId={userId}&code={code}";
        }

        private string GetPasswordResetUrl(string userId, string code)
        {
            return $"{_siteUrl}/account/reset-password?userId={userId}&code={code}";
        }
    }
}
