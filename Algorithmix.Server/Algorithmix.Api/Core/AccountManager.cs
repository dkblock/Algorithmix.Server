﻿using Algorithmix.Identity;
using Algorithmix.Mappers;
using Algorithmix.Models.Account;
using Algorithmix.Models.Users;
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

        public async Task<UserAccount> Authenticate()
        {
            var authorization = _userContextManager.Authorization;
            var authModel = _authService.CheckAuth(authorization);
            var user = await _userManager.GetUserById(authModel.CurrentUser.Id);

            return _userMapper.ToModel(user, authModel.AccessToken);
        }

        public async Task<UserAccount> Register(RegisterPayload registerPayload)
        {
            var user = await _userManager.CreateUser(registerPayload);
            var accessToken = _authService.Authenticate(user);

            var code = await _userManager.GenerateEmailConfirmationToken(user.Id);
            var callbackUrl = GetEmailConfirmationUrl(user.Id, HttpUtility.UrlEncode(code));

            await _emailManager.SendEmail(user.Email, "Регистрация в Algorithmix",
                $"Здравствуйте, {user.FirstName} {user.LastName}!<br/>" +
                "Благодарим Вас за регистрацию на сайте Algorithmix.<br/>" +
                $"Чтобы подтвердить свой адрес электронной почты, <a href='{callbackUrl}'>перейдите по этой ссылке</a>.<br/>" +
                "Если вы получили данное письмо по ошибке, пожалуйста, сообщите об этом отправителю и удалите это сообщение.");

            return _userMapper.ToModel(user, accessToken);
        }

        public async Task<UserAccount> Login(LoginPayload loginPayload)
        {
            var user = await _userManager.GetUserByEmail(loginPayload.Email);
            var accessToken = _authService.Authenticate(user);

            return _userMapper.ToModel(user, accessToken);
        }

        public async Task<UserAccount> UpdateUserInformation(ApplicationUserPayload userPayload)
        {
            var userId = _userContextManager.CurrentUser.Id;
            var updatedUser = await _userManager.UpdateUser(userId, userPayload);

            return new UserAccount
            {
                CurrentUser = updatedUser,
                AccessToken = _userContextManager.AccessToken
            };
        }

        public async Task ConfirmEmailRequest()
        {
            var userId = _userContextManager.CurrentUser.Id;
            var user = await _userManager.GetUserById(userId);
            var code = await _userManager.GenerateEmailConfirmationToken(user.Id);
            var callbackUrl = GetEmailConfirmationUrl(user.Id, HttpUtility.UrlEncode(code));

            await _emailManager.SendEmail(user.Email, "Подтверждение e-mail адреса",
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
            var userId = _userContextManager.CurrentUser.Id;
            return await _userManager.ChangePassword(userId, changePasswordPayload);
        }

        public async Task ResetPasswordRequest(string email)
        {
            var user = await _userManager.GetUserByEmail(email);
            var code = await _userManager.GeneratePasswordResetToken(user.Id);
            var callbackUrl = GetPasswordResetUrl(user.Id, HttpUtility.UrlEncode(code));

            await _emailManager.SendEmail(email, "Сброс пароля",
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
            return $"{_siteUrl}/account/confirm-email?userId={userId}&code={code}";
        }

        private string GetPasswordResetUrl(string userId, string code)
        {
            return $"{_siteUrl}/account/reset-password?userId={userId}&code={code}";
        }
    }
}
