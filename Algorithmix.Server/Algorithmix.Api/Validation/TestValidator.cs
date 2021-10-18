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

            var algorithmValidationErrors = await ValidateAlgorithms(test);
            validationErrors.AddRange(algorithmValidationErrors);

            return new ValidationResult
            {
                IsValid = !validationErrors.Any(),
                ValidationErrors = validationErrors
            };
        }

        private async Task<IEnumerable<ValidationError>> ValidateAlgorithms(TestPayload test)
        {
            var validationErrors = new List<ValidationError>();

            foreach(var algorithmId in test.AlgorithmIds)
            {
                var algorithm = await _algorithmService.GetAlgorithm(algorithmId);

                if (algorithm == null)
                    validationErrors.Add(new ValidationError
                    {
                        Field = nameof(test.AlgorithmIds),
                        Message = $"Алгоритма с данным ID ({algorithm.Id}) не существует"
                    });
            }

            return validationErrors;
        }
    }
}
