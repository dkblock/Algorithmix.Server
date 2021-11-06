using Algorithmix.Common.Extensions;
using Algorithmix.Common.Validation;
using Algorithmix.Models.Algorithms;
using Algorithmix.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Algorithmix.Api.Validation
{
    public class AlgorithmValidator
    {
        private readonly AlgorithmService _algorithmService;

        public AlgorithmValidator(AlgorithmService algorithmService)
        {
            _algorithmService = algorithmService;
        }

        public async Task<ValidationResult> Validate(AlgorithmPayload algorithm)
        {
            var validationErrors = new List<ValidationError>();
            var algorithms = await _algorithmService.GetAllAlgorithms();

            if (string.IsNullOrEmpty(algorithm.Id))
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(algorithm.Id).ToCamelCase(),
                    Message = "Введите ID"
                });

            if (algorithm.Id.Contains(' '))
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(algorithm.Id).ToCamelCase(),
                    Message = "ID алгоритма не должен содержать пробелы"
                });

            if (algorithms.Any(a => a.Id == algorithm.Id))
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(algorithm.Id).ToCamelCase(),
                    Message = "Алгоритм с таким ID уже существует"
                });

            if (string.IsNullOrEmpty(algorithm.Name))
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(algorithm.Name).ToCamelCase(),
                    Message = "Введите название"
                });

            if (algorithms.Any(a => a.Name == algorithm.Name))
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(algorithm.Name).ToCamelCase(),
                    Message = "Алгоритм с таким названием уже существует"
                });

            return new ValidationResult
            {
                IsValid = !validationErrors.Any(),
                ValidationErrors = validationErrors
            };
        }
    }
}
