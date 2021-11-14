using Algorithmix.Common.Extensions;
using Algorithmix.Common.Validation;
using Algorithmix.Models.Account;
using Algorithmix.Models.Users;
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

            validationErrors.AddRange(await ValidateEmail(registerPayload.Email));
            validationErrors.AddRange(ValidateName(registerPayload.FirstName, registerPayload.LastName));
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

        public async Task<ValidationResult> ValidateOnUpdate(string userId, ApplicationUserPayload userPayload)
        {
            var validationErrors = new List<ValidationError>();

            validationErrors.AddRange(await ValidateEmail(userPayload.Email, userId));
            validationErrors.AddRange(ValidateName(userPayload.FirstName, userPayload.LastName));

            return new ValidationResult
            {
                IsValid = !validationErrors.Any(),
                ValidationErrors = validationErrors
            };
        }

        public async Task<ValidationResult> ValidateOnChangePassword(string userId, ChangePasswordPayload changePasswordPayload)
        {
            var validationErrors = new List<ValidationError>();
            var user = await _userService.GetUserById(userId);

            if (!await _userService.IsEmailConfirmed(userId))
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(changePasswordPayload.CurrentPassword).ToCamelCase(),
                    Message = "Ваш e-mail не подтверждён"
                });

            if (!await _userService.IsPasswordValid(user.Email, changePasswordPayload.CurrentPassword))
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(changePasswordPayload.CurrentPassword).ToCamelCase(),
                    Message = "Неверный пароль"
                });

            if (await _userService.IsPasswordValid(user.Email, changePasswordPayload.NewPassword))
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(changePasswordPayload.NewPassword).ToCamelCase(),
                    Message = "Новый пароль должен отличаться от старого"
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

        private async Task<IEnumerable<ValidationError>> ValidateEmail(string email, string userId = null)
        {
            var validationErrors = new List<ValidationError>();

            if (string.IsNullOrEmpty(email))
                validationErrors.Add(new ValidationError
                {
                    Field = "email",
                    Message = "Введите Email"
                });

            if (!Regex.IsMatch(email, EmailPattern))
                validationErrors.Add(new ValidationError
                {
                    Field = "email",
                    Message = "Неверный формат Email"
                });

            var userByEmail = await _userService.GetUserByEmail(email);

            if (userByEmail != null && userByEmail.Id != userId)
                validationErrors.Add(new ValidationError
                {
                    Field = "email",
                    Message = "Пользователь с данным Email уже существует"
                });

            return validationErrors;
        }

        private IEnumerable<ValidationError> ValidateName(string firstName, string lastName)
        {
            var validationErrors = new List<ValidationError>();

            if (string.IsNullOrEmpty(firstName))
                validationErrors.Add(new ValidationError
                {
                    Field = "firstName",
                    Message = "Введите имя"
                });

            if (firstName.Length > 50)
                validationErrors.Add(new ValidationError
                {
                    Field = "firstName",
                    Message = "Длина не должна превышать 50 символов"
                });

            if (string.IsNullOrEmpty(lastName))
                validationErrors.Add(new ValidationError
                {
                    Field = "lastName",
                    Message = "Введите фамилию"
                });

            if (lastName.Length > 50)
                validationErrors.Add(new ValidationError
                {
                    Field = "lastName",
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
