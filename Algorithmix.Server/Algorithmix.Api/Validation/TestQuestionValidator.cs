using Algorithmix.Common.Validation;
using Algorithmix.Models.Tests;
using Algorithmix.Services.TestDesign;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Algorithmix.Api.Validation
{
    public class TestQuestionValidator
    {
        private readonly TestService _testService;

        public TestQuestionValidator(TestService testService)
        {
            _testService = testService;
        }

        public async Task<ValidationResult> Validate(TestQuestionPayload question)
        {
            var validationErrors = new List<ValidationError>();

            if (string.IsNullOrEmpty(question.Value))
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(question.Value),
                    Message = "Введите вопрос"
                });

            var test = await _testService.GetTest(question.TestId);

            if (test == null)
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(question.TestId),
                    Message = "Теста с данным ID не существует"
                });

            return new ValidationResult
            {
                IsValid = !validationErrors.Any(),
                ValidationErrors = validationErrors
            };
        }
    }
}
