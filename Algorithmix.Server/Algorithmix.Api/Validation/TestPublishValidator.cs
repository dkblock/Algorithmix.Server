using Algorithmix.Common.Constants;
using Algorithmix.Common.Extensions;
using Algorithmix.Common.Validation;
using Algorithmix.Models.Tests;
using System.Collections.Generic;
using System.Linq;

namespace Algorithmix.Api.Validation
{
    public class TestPublishValidator
    {
        public TestPublishValidator() { }

        public ValidationResult Validate(Test test)
        {
            var validationErrors = new List<ValidationError>();
            var questions = test.Questions.ToList();
            var answers = questions.Select(q => q.Answers).ToList();

            validationErrors.AddRange(ValidateTest(test));
            validationErrors.AddRange(ValidateQuestions(questions));
            validationErrors.AddRange(ValidateAnswers(answers));

            return new ValidationResult
            {
                IsValid = !validationErrors.Any(),
                ValidationErrors = validationErrors
            };
        }

        private IEnumerable<ValidationError> ValidateTest(Test test)
        {
            var validationErrors = new List<ValidationError>();

            if (string.IsNullOrEmpty(test.Name))
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(test.Name).ToCamelCase(),
                    Message = "Название теста не может быть пустым"
                });

            if (test.Name.Length > 50)
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(test.Name).ToCamelCase(),
                    Message = "Название теста должно содержать не более 50 символов"
                });

            if (!test.Questions.Any())
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(test.Questions).ToCamelCase(),
                    Message = "Тест должен содержать хотя бы один вопрос"
                });

            if (!test.Algorithms.Any())
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(test.Algorithms).ToCamelCase(),
                    Message = "Тест должен относиться хотя бы к одному алгоритму"
                });

            return validationErrors;
        }

        private IEnumerable<ValidationError> ValidateQuestions(List<TestQuestion> questions)
        {
            var validationErrors = new List<ValidationError>();

            for (int i = 0; i < questions.Count; i++)
            {
                var question = questions[i];
                var questionErrors = ValidateQuestion(question, i + 1, questions.Count);

                validationErrors.AddRange(questionErrors);
            }

            return validationErrors;
        }

        private IEnumerable<ValidationError> ValidateQuestion(TestQuestion question, int questionOrder, int questionsCount)
        {
            var validationErrors = new List<ValidationError>();

            if (string.IsNullOrEmpty(question.Value))
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(question.Value).ToCamelCase(),
                    Message = $"Вопрос #{questionOrder} не может быть пустым"
                });

            if (questionOrder == 1 && question.PreviousQuestionId != null)
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(question.PreviousQuestionId).ToCamelCase(),
                    Message = "Первый вопрос не может иметь предка. Обратитесь к администратору"
                });

            if (questionOrder == questionsCount && question.NextQuestionId != null)
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(question.NextQuestionId).ToCamelCase(),
                    Message = "Последний вопрос не может иметь потомка. Обратитесь к администратору"
                });

            if (!question.Answers.Any())
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(question.Answers).ToCamelCase(),
                    Message = $"Вопрос #{questionOrder} должен содержать хотя бы один ответ"
                });

            var correctAnswers = question.Answers.Where(a => a.IsCorrect);

            if (question.Answers.Any() && !correctAnswers.Any())
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(question.Answers).ToCamelCase(),
                    Message = $"Вопрос #{questionOrder} должен содержать хотя бы один правильный ответ"
                });

            if (correctAnswers.Count() > 1 && (question.Type == TestQuestionTypes.SingleAnswerQuestion || question.Type == TestQuestionTypes.FreeAnswerQuestion))
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(question.Answers).ToCamelCase(),
                    Message = $"Вопрос #{questionOrder} не может содержать более одного правильного ответа"
                });

            return validationErrors;
        }

        private IEnumerable<ValidationError> ValidateAnswers(List<IEnumerable<TestAnswer>> answers)
        {
            var validationErrors = new List<ValidationError>();

            for (int i = 0; i < answers.Count; i++)
            {
                var questionAnswers = answers[i].ToList();

                for (int j = 0; j < questionAnswers.Count; j++)
                {
                    var answer = questionAnswers[j];
                    var answerErrors = ValidateAnswer(answer, i + 1, j + 1, questionAnswers.Count);

                    validationErrors.AddRange(answerErrors);
                }
            }

            return validationErrors;
        }

        private IEnumerable<ValidationError> ValidateAnswer(TestAnswer answer, int questionOrder, int answerOrder, int answersCount)
        {
            var validationErrors = new List<ValidationError>();

            if (string.IsNullOrEmpty(answer.Value))
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(answer.Value).ToCamelCase(),
                    Message = $"Вопрос #{questionOrder}. Ответ #{answerOrder} не может быть пустым"
                });

            if (answerOrder == 1 && answer.PreviousAnswerId != null)
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(answer.PreviousAnswerId).ToCamelCase(),
                    Message = $"Вопрос #{questionOrder}. Первый ответ не может иметь предка. Обратитесь к администратору"
                });

            if (answerOrder == answersCount && answer.NextAnswerId != null)
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(answer.NextAnswerId).ToCamelCase(),
                    Message = $"Вопрос #{questionOrder}. Последний ответ не может иметь потомка. Обратитесь к администратору"
                });

            return validationErrors;
        }
    }
}
