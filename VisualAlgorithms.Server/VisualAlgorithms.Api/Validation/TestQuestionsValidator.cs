using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisualAlgorithms.Common.Validation;
using VisualAlgorithms.Models.Tests;
using VisualAlgorithms.Services;

namespace VisualAlgorithms.Api.Validation
{
    public class TestQuestionsValidator
    {
        private readonly TestAnswersService _answersService;
        private readonly TestsService _testsService;

        public TestQuestionsValidator(TestAnswersService answersService, TestsService testsService)
        {
            _answersService = answersService;
            _testsService = testsService;
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

            var answer = await _answersService.GetTestAnswer(question.CorrectAnswerId);
            var test = await _testsService.GetTest(question.TestId);

            if (answer == null)
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(question.CorrectAnswerId),
                    Message = "Ответа с данным ID не существует"
                });

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
