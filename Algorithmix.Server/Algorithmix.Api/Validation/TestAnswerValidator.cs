using Algorithmix.Common.Validation;
using Algorithmix.Models.Tests;
using Algorithmix.Services.TestDesign;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Algorithmix.Api.Validation
{
    public class TestAnswerValidator
    {
        private readonly TestQuestionService _questionService;

        public TestAnswerValidator(TestQuestionService questionService)
        {
            _questionService = questionService;
        }

        public async Task<ValidationResult> Validate(TestAnswerPayload answer)
        {
            var validationErrors = new List<ValidationError>();

            if (string.IsNullOrEmpty(answer.Value))
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(answer.Value),
                    Message = "Введите ответ"
                });

            var question = await _questionService.GetTestQuestion(answer.QuestionId);

            if (question == null)
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(answer.QuestionId),
                    Message = "Вопроса с данным ID не существует"
                });

            return new ValidationResult
            {
                IsValid = !validationErrors.Any(),
                ValidationErrors = validationErrors
            };
        }
    }
}
