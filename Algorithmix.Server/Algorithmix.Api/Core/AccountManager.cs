using Algorithmix.Configuration;
using Algorithmix.Identity.Core;
using Algorithmix.Mappers;
using Algorithmix.Models.Account;
using Algorithmix.Models.Users;
using System.Threading.Tasks;
using System.Web;

namespace Algorithmix.Api.Core
{
    public class AccountManager
    {
        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationUserMapper _userMapper;
        private readonly AuthenticationService _authService;
        private readonly MailManager _mailManager;
        private readonly IUserContextHandler _userContextHandler;

        private readonly string _clientUrl;

        public AccountManager(
            ApplicationUserManager userManager,
            ApplicationUserMapper userMapper,
            AuthenticationService authService,
            MailManager mailManager,
            IUserContextHandler userContextHandler,
            IConfig configuration)
        {
            _userManager = userManager;
            _userMapper = userMapper;
            _authService = authService;
            _mailManager = mailManager;
            _userContextHandler = userContextHandler;

            _clientUrl = configuration.ClientUrl;
        }

        public async Task<UserAccount> Authenticate()
        {
            if (_userContextHandler.AccessToken == null || _userContextHandler.CurrentUser == null)
                return null;

            var user = await _userManager.GetUserById(_userContextHandler.CurrentUser.Id);
            return _userMapper.ToModel(user, _userContextHandler.AccessToken);
        }

        public async Task<UserAccount> Register(RegisterPayload registerPayload)
        {
            var user = await _userManager.CreateUser(registerPayload);
            var code = await _userManager.GenerateEmailConfirmationToken(user.Id);
            var callbackUrl = GetEmailConfirmationUrl(user.Id, HttpUtility.UrlEncode(code));

            await _mailManager.SendEmail(user.Email, "Регистрация в Algorithmix",
                $"Здравствуйте, {user.FirstName} {user.LastName}!<br/>" +
                "Благодарим Вас за регистрацию на сайте Algorithmix.<br/>" +
                $"Чтобы подтвердить свой адрес электронной почты, <a href='{callbackUrl}'>перейдите по этой ссылке</a>.<br/>" +
                "Если вы получили данное письмо по ошибке, пожалуйста, сообщите об этом отправителю и удалите это сообщение.");

            var authUser = _authService.Authenticate(user);
            _userContextHandler.AttachUser(authUser);

            return authUser;
        }

        public async Task<UserAccount> Login(LoginPayload loginPayload)
        {
            var user = await _userManager.GetUserByEmail(loginPayload.Email);
            var authUser = _authService.Authenticate(user);
            _userContextHandler.AttachUser(authUser);

            return authUser;
        }

        public async Task<UserAccount> UpdateUserInformation(ApplicationUserPayload userPayload)
        {
            var userId = _userContextHandler.CurrentUser.Id;
            var updatedUser = await _userManager.UpdateUser(userId, userPayload);

            return _userMapper.ToModel(updatedUser, _userContextHandler.AccessToken);
        }

        public async Task ConfirmEmailRequest()
        {
            var userId = _userContextHandler.CurrentUser.Id;
            var user = await _userManager.GetUserById(userId);
            var code = await _userManager.GenerateEmailConfirmationToken(user.Id);
            var callbackUrl = GetEmailConfirmationUrl(user.Id, HttpUtility.UrlEncode(code));

            await _mailManager.SendEmail(user.Email, "Подтверждение e-mail адреса",
                $"Здравствуйте, {user.FirstName} {user.LastName}!<br/>" +
                $"Чтобы подтвердить свой адрес электронной почты, <a href='{callbackUrl}'>перейдите по этой ссылке</a>.<br/>" +
                "Если вы получили данное письмо по ошибке, пожалуйста, сообщите об этом отправителю и удалите это сообщение.");
        }

        public async Task<bool> ConfirmEmail(ConfirmEmailPayload confirmEmailPayload)
        {
            return await _userManager.ConfirmEmail(confirmEmailPayload.UserId, confirmEmailPayload.Code);
        }

        public async Task<bool> ChangePassword(ChangePasswordPayload changePasswordPayload)
        {
            var userId = _userContextHandler.CurrentUser.Id;
            return await _userManager.ChangePassword(userId, changePasswordPayload);
        }

        public async Task ResetPasswordRequest(string email)
        {
            var user = await _userManager.GetUserByEmail(email);
            var code = await _userManager.GeneratePasswordResetToken(user.Id);
            var callbackUrl = GetPasswordResetUrl(user.Id, HttpUtility.UrlEncode(code));

            await _mailManager.SendEmail(email, "Сброс пароля",
                $"Здравствуйте, {user.FirstName} {user.LastName}!<br/>" +
                $"Чтобы сбросить пароль, <a href='{callbackUrl}'>перейдите по этой ссылке</a>.<br/>" +
                "Если вы получили данное письмо по ошибке, пожалуйста, сообщите об этом отправителю и удалите это сообщение.");
        }

        public async Task<bool> ResetPassword(string userId, ResetPasswordPayload resetPasswordPayload)
        {
            return await _userManager.ResetPassword(userId, resetPasswordPayload);
        }

        private string GetEmailConfirmationUrl(string userId, string code)
        {
            return $"{_clientUrl}/account/confirm-email?userId={userId}&code={code}";
        }

        private string GetPasswordResetUrl(string userId, string code)
        {
            return $"{_clientUrl}/account/reset-password?userId={userId}&code={code}";
        }
    }
}
