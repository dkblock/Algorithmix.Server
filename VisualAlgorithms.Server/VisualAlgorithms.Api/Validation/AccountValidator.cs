using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VisualAlgorithms.Common.Validation;
using VisualAlgorithms.Models.Account;
using VisualAlgorithms.Services;

namespace VisualAlgorithms.Api.Validation
{
    public class AccountValidator
    {
        private const string EmailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";

        private readonly UserService _usersService;

        public AccountValidator(UserService usersService)
        {
            _usersService = usersService;
        }

        public async Task<ValidationResult> Validate(LoginModel loginModel)
        {
            var validationErrors = new List<ValidationError>();

            if (!await _usersService.IsPasswordValid(loginModel.Email, loginModel.Password))
                validationErrors.Add(new ValidationError
                {
                    Field = $"{nameof(loginModel.Email)} {nameof(loginModel.Password)}",
                    Message = "Неверный логин и (или) пароль"
                });

            return new ValidationResult
            {
                IsValid = !validationErrors.Any(),
                ValidationErrors = validationErrors
            };
        }

        public async Task<ValidationResult> Validate(RegisterModel registerModel)
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

        private async Task<IEnumerable<ValidationError>> ValidateEmail(RegisterModel registerModel)
        {
            var validationErrors = new List<ValidationError>();

            if (string.IsNullOrEmpty(registerModel.Email))
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(registerModel.Email),
                    Message = "Введите email"
                });

            if (!Regex.IsMatch(registerModel.Email, EmailPattern))
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(registerModel.Email),
                    Message = "Неверный формат email"
                });

            if (await _usersService.GetUserByEmail(registerModel.Email) != null)
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(registerModel.Email),
                    Message = "Пользователь с данным email уже существует"
                });

            return validationErrors;
        }

        private IEnumerable<ValidationError> ValidateName(RegisterModel registerModel)
        {
            var validationErrors = new List<ValidationError>();

            if (string.IsNullOrEmpty(registerModel.FirstName))
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(registerModel.FirstName),
                    Message = "Введите имя"
                });

            if (registerModel.FirstName.Length > 50)
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(registerModel.FirstName),
                    Message = "Длина не должна превышать 50 символов"
                });

            if (string.IsNullOrEmpty(registerModel.LastName))
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(registerModel.LastName),
                    Message = "Введите фамилию"
                });

            if (registerModel.LastName.Length > 50)
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(registerModel.LastName),
                    Message = "Длина не должна превышать 50 символов"
                });

            return validationErrors;
        }

        private IEnumerable<ValidationError> ValidatePassword(RegisterModel registerModel)
        {
            var validationErrors = new List<ValidationError>();

            if (string.IsNullOrEmpty(registerModel.Password))
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(registerModel.Password),
                    Message = "Введите пароль"
                });

            if (registerModel.Password.Length < 6)
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(registerModel.Password),
                    Message = "Минимальная длина составляет 6 символов"
                });

            if (registerModel.Password.Length > 50)
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(registerModel.Password),
                    Message = "Длина не должна превышать 50 символов"
                });

            return validationErrors;
        }
    }
}
