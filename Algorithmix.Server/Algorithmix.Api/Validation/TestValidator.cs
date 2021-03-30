using Algorithmix.Common.Validation;
using Algorithmix.Models.Tests;
using Algorithmix.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Algorithmix.Api.Validation
{
    public class TestValidator
    {
        private readonly AlgorithmService _algorithmService;

        public TestValidator(AlgorithmService algorithmService)
        {
            _algorithmService = algorithmService;
        }

        public async Task<ValidationResult> Validate(TestPayload test)
        {
            var validationErrors = new List<ValidationError>();

            if (string.IsNullOrEmpty(test.Name))
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(test.Name),
                    Message = "Введите название теста"
                });

            if (test.Name.Length > 50)
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(test.Name),
                    Message = "Название теста должно содержать не более 50 символов"
                });

            var algorithm = await _algorithmService.GetAlgorithm(test.AlgorithmId);

            if (algorithm == null)
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(test.AlgorithmId),
                    Message = "Алгоритма с данным ID не существует"
                });

            return new ValidationResult
            {
                IsValid = !validationErrors.Any(),
                ValidationErrors = validationErrors
            };
        }
    }
}
