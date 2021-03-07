using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisualAlgorithms.Common.Validation;
using VisualAlgorithms.Domain;
using VisualAlgorithms.Services;

namespace VisualAlgorithms.Server.Validation
{
    public class TestAnswersValidator
    {
        private readonly TestQuestionsService _questionsService;

        public TestAnswersValidator(TestQuestionsService questionsService)
        {
            _questionsService = questionsService;
        }

        public async Task<ValidationResult> Validate(TestAnswer answer)
        {
            var validationErrors = new List<ValidationError>();

            if (string.IsNullOrEmpty(answer.Value))
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(answer.Value),
                    Message = "Введите ответ"
                });

            var question = await _questionsService.GetTestQuestion(answer.Id);

            if (question == null)
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(answer.TestQuestionId),
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
