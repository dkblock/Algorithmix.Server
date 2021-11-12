using Algorithmix.Common.Extensions;
using Algorithmix.Common.Validation;
using Algorithmix.Models.Account;
using Algorithmix.Services;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Algorithmix.Api.Validation
{
    public class AccountValidator
    {
        private const string EmailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";

        private readonly ApplicationUserService _userService;

        public AccountValidator(ApplicationUserService userService)
        {
            _userService = userService;
        }

        public async Task<ValidationResult> ValidateOnLogin(LoginPayload loginPayload)
        {
            var validationErrors = new List<ValidationError>();

            if (!await _userService.IsPasswordValid(loginPayload.Email, loginPayload.Password))
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(loginPayload.Password).ToCamelCase(),
                    Message = "Неверный логин и (или) пароль"
                });

            return new ValidationResult
            {
                IsValid = !validationErrors.Any(),
                ValidationErrors = validationErrors
            };
        }

        public async Task<ValidationResult> ValidateOnRegister(RegisterPayload registerPayload)
        {
            var validationErrors = new List<ValidationError>();

            validationErrors.AddRange(await ValidateEmail(registerPayload));
            validationErrors.AddRange(ValidateName(registerPayload));
            validationErrors.AddRange(ValidatePasswords(
                    registerPayload.Password,
                    registerPayload.ConfirmPassword,
                    nameof(registerPayload.Password),
                    nameof(registerPayload.ConfirmPassword)));

            return new ValidationResult
            {
                IsValid = !validationErrors.Any(),
                ValidationErrors = validationErrors
            };
        }

        public async Task<ValidationResult> ValidateOnChangePassword(string userId, ChangePasswordPayload changePasswordPayload)
        {
            var validationErrors = new List<ValidationError>();

            if (!await _userService.IsEmailConfirmed(userId))
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(changePasswordPayload.CurrentPassword).ToCamelCase(),
                    Message = "Ваш e-mail не подтверждён"
                });

            if (!await _userService.IsPasswordValid(userId, changePasswordPayload.CurrentPassword))
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(changePasswordPayload.CurrentPassword).ToCamelCase(),
                    Message = "Неверный пароль"
                });

            validationErrors.AddRange(ValidatePasswords(
                changePasswordPayload.NewPassword,
                changePasswordPayload.ConfirmNewPassword,
                nameof(changePasswordPayload.NewPassword),
                nameof(changePasswordPayload.ConfirmNewPassword)));

            return new ValidationResult
            {
                IsValid = !validationErrors.Any(),
                ValidationErrors = validationErrors
            };
        }

        public async Task<ValidationResult> ValidateOnPasswordResetRequest(ResetPasswordPayload resetPasswordPayload)
        {
            var validationErrors = new List<ValidationError>();
            var user = await _userService.GetUserByEmail(resetPasswordPayload.Email);

            if (user == null)
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(resetPasswordPayload.Email).ToCamelCase(),
                    Message = "Пользователь с данным e-mail не найден"
                });
            else if (!await _userService.IsEmailConfirmed(user.Id))
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(resetPasswordPayload.Email).ToCamelCase(),
                    Message = "Данный e-mail адрес не подтверждён"
                });

            return new ValidationResult
            {
                IsValid = !validationErrors.Any(),
                ValidationErrors = validationErrors
            };
        }

        public ValidationResult ValidateOnPasswordReset(ResetPasswordPayload resetPasswordPayload)
        {
            var validationErrors = new List<ValidationError>();

            validationErrors.AddRange(ValidatePasswords(
                resetPasswordPayload.NewPassword,
                resetPasswordPayload.ConfirmNewPassword,
                nameof(resetPasswordPayload.NewPassword),
                nameof(resetPasswordPayload.ConfirmNewPassword)));

            return new ValidationResult
            {
                IsValid = !validationErrors.Any(),
                ValidationErrors = validationErrors
            };
        }

        private async Task<IEnumerable<ValidationError>> ValidateEmail(RegisterPayload registerPayload)
        {
            var validationErrors = new List<ValidationError>();

            if (string.IsNullOrEmpty(registerPayload.Email))
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(registerPayload.Email).ToCamelCase(),
                    Message = "Введите Email"
                });

            if (!Regex.IsMatch(registerPayload.Email, EmailPattern))
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(registerPayload.Email).ToCamelCase(),
                    Message = "Неверный формат Email"
                });

            if (await _userService.GetUserByEmail(registerPayload.Email) != null)
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(registerPayload.Email).ToCamelCase(),
                    Message = "Пользователь с данным Email уже существует"
                });

            return validationErrors;
        }

        private IEnumerable<ValidationError> ValidateName(RegisterPayload registerPayload)
        {
            var validationErrors = new List<ValidationError>();

            if (string.IsNullOrEmpty(registerPayload.FirstName))
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(registerPayload.FirstName).ToCamelCase(),
                    Message = "Введите имя"
                });

            if (registerPayload.FirstName.Length > 50)
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(registerPayload.FirstName).ToCamelCase(),
                    Message = "Длина не должна превышать 50 символов"
                });

            if (string.IsNullOrEmpty(registerPayload.LastName))
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(registerPayload.LastName).ToCamelCase(),
                    Message = "Введите фамилию"
                });

            if (registerPayload.LastName.Length > 50)
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(registerPayload.LastName).ToCamelCase(),
                    Message = "Длина не должна превышать 50 символов"
                });

            return validationErrors;
        }

        private IEnumerable<ValidationError> ValidatePasswords(string password, string confirmPassword, string nameOfPassword, string nameOfConfirmPassword)
        {
            var validationErrors = new List<ValidationError>();

            if (string.IsNullOrEmpty(password))
                validationErrors.Add(new ValidationError
                {
                    Field = nameOfPassword.ToCamelCase(),
                    Message = "Введите пароль"
                });

            if (password.Length < 6)
                validationErrors.Add(new ValidationError
                {
                    Field = nameOfPassword.ToCamelCase(),
                    Message = "Минимальная длина составляет 6 символов"
                });

            if (password.Length > 50)
                validationErrors.Add(new ValidationError
                {
                    Field = nameOfPassword.ToCamelCase(),
                    Message = "Длина не должна превышать 50 символов"
                });

            if (string.IsNullOrEmpty(confirmPassword))
                validationErrors.Add(new ValidationError
                {
                    Field = nameOfConfirmPassword.ToCamelCase(),
                    Message = "Введите пароль"
                });

            if (password != confirmPassword)
                validationErrors.Add(new ValidationError
                {
                    Field = nameOfConfirmPassword.ToCamelCase(),
                    Message = "Пароли не совпадают"
                });

            return validationErrors;
        }
    }
}
