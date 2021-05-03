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

        public async Task<ValidationResult> Validate(LoginPayload loginModel)
        {
            var validationErrors = new List<ValidationError>();

            if (!await _userService.IsPasswordValid(loginModel.Email, loginModel.Password))
            {
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(loginModel.Email).ToCamelCase(),
                    Message = " "
                });

                validationErrors.Add(new ValidationError
                {
                    Field = nameof(loginModel.Password).ToCamelCase(),
                    Message = "Неверный логин и (или) пароль"
                });
            }

            return new ValidationResult
            {
                IsValid = !validationErrors.Any(),
                ValidationErrors = validationErrors
            };
        }

        public async Task<ValidationResult> Validate(RegisterPayload registerModel)
        {
            var validationErrors = new List<ValidationError>();

            validationErrors.AddRange(await ValidateEmail(registerModel));
            validationErrors.AddRange(ValidateName(registerModel));
            validationErrors.AddRange(ValidatePassword(registerModel));

            return new ValidationResult
            {
                IsValid = !validationErrors.Any(),
                ValidationErrors = validationErrors
            };
        }

        private async Task<IEnumerable<ValidationError>> ValidateEmail(RegisterPayload registerModel)
        {
            var validationErrors = new List<ValidationError>();

            if (string.IsNullOrEmpty(registerModel.Email))
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(registerModel.Email).ToCamelCase(),
                    Message = "Введите Email"
                });

            if (!Regex.IsMatch(registerModel.Email, EmailPattern))
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(registerModel.Email).ToCamelCase(),
                    Message = "Неверный формат Email"
                });

            if (await _userService.GetUserByEmail(registerModel.Email) != null)
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(registerModel.Email).ToCamelCase(),
                    Message = "Пользователь с данным Email уже существует"
                });

            return validationErrors;
        }

        private IEnumerable<ValidationError> ValidateName(RegisterPayload registerModel)
        {
            var validationErrors = new List<ValidationError>();

            if (string.IsNullOrEmpty(registerModel.FirstName))
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(registerModel.FirstName).ToCamelCase(),
                    Message = "Введите имя"
                });

            if (registerModel.FirstName.Length > 50)
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(registerModel.FirstName).ToCamelCase(),
                    Message = "Длина не должна превышать 50 символов"
                });

            if (string.IsNullOrEmpty(registerModel.LastName))
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(registerModel.LastName).ToCamelCase(),
                    Message = "Введите фамилию"
                });

            if (registerModel.LastName.Length > 50)
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(registerModel.LastName).ToCamelCase(),
                    Message = "Длина не должна превышать 50 символов"
                });

            return validationErrors;
        }

        private IEnumerable<ValidationError> ValidatePassword(RegisterPayload registerModel)
        {
            var validationErrors = new List<ValidationError>();

            if (string.IsNullOrEmpty(registerModel.Password))
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(registerModel.Password).ToCamelCase(),
                    Message = "Введите пароль"
                });

            if (registerModel.Password.Length < 6)
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(registerModel.Password).ToCamelCase(),
                    Message = "Минимальная длина составляет 6 символов"
                });

            if (registerModel.Password.Length > 50)
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(registerModel.Password).ToCamelCase(),
                    Message = "Длина не должна превышать 50 символов"
                });

            if (string.IsNullOrEmpty(registerModel.ConfirmPassword))
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(registerModel.ConfirmPassword).ToCamelCase(),
                    Message = "Введите пароль"
                });

            if (registerModel.ConfirmPassword.Length < 6)
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(registerModel.ConfirmPassword).ToCamelCase(),
                    Message = "Минимальная длина составляет 6 символов"
                });

            if (registerModel.ConfirmPassword.Length > 50)
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(registerModel.ConfirmPassword).ToCamelCase(),
                    Message = "Длина не должна превышать 50 символов"
                });

            if (registerModel.Password != registerModel.ConfirmPassword)
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(registerModel.ConfirmPassword).ToCamelCase(),
                    Message = "Пароли не совпадают"
                });

            return validationErrors;
        }
    }
}
