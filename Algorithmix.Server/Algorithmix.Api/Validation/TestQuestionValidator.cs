using Algorithmix.Common.Validation;
using Algorithmix.Models.Tests;
using Algorithmix.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Algorithmix.Api.Validation
{
    public class TestQuestionValidator
    {
        private readonly TestAnswerService _answerService;
        private readonly TestService _testService;

        public TestQuestionValidator(TestAnswerService answerService, TestService testService)
        {
            _answerService = answerService;
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

            var answer = await _answerService.GetTestAnswer(question.CorrectAnswerId);
            var test = await _testService.GetTest(question.TestId);

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
