using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisualAlgorithms.Common.Validation;
using VisualAlgorithms.Domain;
using VisualAlgorithms.Services;

namespace VisualAlgorithms.Server.Validation
{
    public class TestQuestionsValidator
    {
        private readonly TestsService _testsService;

        public TestQuestionsValidator(TestsService testsService)
        {
            _testsService = testsService;
        }

        public async Task<ValidationResult> Validate(TestQuestion question)
        {
            var validationErrors = new List<ValidationError>();

            if (string.IsNullOrEmpty(question.Value))
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(question.Value),
                    Message = "Введите вопрос"
                });

            return new ValidationResult
            {
                IsValid = !validationErrors.Any(),
                ValidationErrors = validationErrors
            };
        }
    }
}
