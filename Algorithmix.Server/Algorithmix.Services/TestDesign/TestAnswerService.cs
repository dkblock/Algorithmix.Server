using Algorithmix.Common.Constants;
using Algorithmix.Common.Extensions;
using Algorithmix.Entities.Test;
using Algorithmix.Mappers;
using Algorithmix.Models.Tests;
using Algorithmix.Repository.TestDesign;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Algorithmix.Services.TestDesign
{
    public class TestAnswerService
    {
        private readonly TestAnswerMapper _answerMapper;
        private readonly TestAnswerRepository _answerRepository;

        public TestAnswerService(TestAnswerMapper answerMapper, TestAnswerRepository answerRepository)
        {
            _answerMapper = answerMapper;
            _answerRepository = answerRepository;
        }

        public async Task<TestAnswer> CreateTestAnswer(TestAnswerPayload answerPayload, string questionType)
        {
            var answers = await _answerRepository.GetTestAnswers(a => a.QuestionId == answerPayload.QuestionId);
            var answerEntity = _answerMapper.ToEntity(answerPayload);
            var createdAnswer = await _answerRepository.CreateTestAnswer(answerEntity);

            if (answers.Any())
            {
                var lastAnswer = answers.Last();
                createdAnswer.PreviousAnswerId = lastAnswer.Id;
                lastAnswer.NextAnswerId = createdAnswer.Id;

                await _answerRepository.UpdateTestAnswer(lastAnswer);
                await _answerRepository.UpdateTestAnswer(createdAnswer);
            }
            else
                await HandleCorrectTestAnswer(createdAnswer, questionType);

            return _answerMapper.ToModel(createdAnswer);
        }

        public async Task<bool> Exists(int answerId, int questionId)
        {
            var answerEntity = await _answerRepository.GetTestAnswerById(answerId);
            return answerEntity != null && answerEntity.QuestionId == questionId;
        }

        public async Task<TestAnswer> GetTestAnswer(int id)
        {
            var answerEntity = await _answerRepository.GetTestAnswerById(id);
            return _answerMapper.ToModel(answerEntity);
        }

        public async Task<IEnumerable<TestAnswer>> GetTestAnswers(int questionId)
        {
            var answerEntities = await _answerRepository.GetTestAnswers(a => a.QuestionId == questionId);
            return _answerMapper.ToModelCollection(answerEntities);
        }

        public async Task<IEnumerable<TestAnswer>> GetTestAnswers(IEnumerable<int> questionIds)
        {
            var answerEntities = await _answerRepository.GetTestAnswers(a => questionIds.Contains(a.QuestionId));
            return _answerMapper.ToModelCollection(answerEntities);
        }

        public async Task DeleteTestAnswer(int id, string questionType)
        {
            var answer = await _answerRepository.GetTestAnswerById(id);
            var questionAnswers = await _answerRepository.GetTestAnswers(a => a.QuestionId == answer.QuestionId);

            if (answer.PreviousAnswerId != null)
            {
                var previousAnswer = await _answerRepository.GetTestAnswerById((int)answer.PreviousAnswerId);
                previousAnswer.NextAnswerId = answer.NextAnswerId;
                await _answerRepository.UpdateTestAnswer(previousAnswer);
            }

            if (answer.NextAnswerId != null)
            {
                var nextQuestion = await _answerRepository.GetTestAnswerById((int)answer.NextAnswerId);
                nextQuestion.PreviousAnswerId = answer.PreviousAnswerId;
                await _answerRepository.UpdateTestAnswer(nextQuestion);
            }

            if (questionType == TestQuestionTypes.SingleAnswerQuestion && answer.IsCorrect)
            {
                var newCorrectAnswer = questionAnswers.FirstOrDefault(a => a.Id != id);

                if (newCorrectAnswer != null)
                {
                    newCorrectAnswer.IsCorrect = true;
                    await _answerRepository.UpdateTestAnswer(newCorrectAnswer);
                }
            }

            await _answerRepository.DeleteTestAnswer(id);
        }

        public async Task<TestAnswer> UpdateTestAnswer(int id, TestAnswerPayload answerPayload, string questionType)
        {
            var answerEntity = _answerMapper.ToEntity(answerPayload, id);

            if (answerEntity.IsCorrect && questionType == TestQuestionTypes.SingleAnswerQuestion)
            {
                var answers = await _answerRepository.GetTestAnswers(a => a.QuestionId == answerEntity.QuestionId);
                var oldCorrectAnswers = answers.Where(a => a.IsCorrect);
                
                foreach(var oldCorrectAnswer in oldCorrectAnswers)
                {
                    if (oldCorrectAnswer.Id != id)
                    {
                        oldCorrectAnswer.IsCorrect = false;
                        await _answerRepository.UpdateTestAnswer(oldCorrectAnswer);
                    }
                }
            }

            var updatedAnswer = await _answerRepository.UpdateTestAnswer(answerEntity);
            return _answerMapper.ToModel(updatedAnswer);
        }

        public async Task UpdateTestAnswers(int questionId, string questionType)
        {
            var answers = await _answerRepository.GetTestAnswers(a => a.QuestionId == questionId);

            if (!answers.Any())
                return;

            await answers.ForEachAsync(async answer =>
            {
                answer.IsCorrect = false;
                await _answerRepository.UpdateTestAnswer(answer);
            });

            await HandleCorrectTestAnswer(answers.First(), questionType);
        }

        public async Task<IEnumerable<TestAnswer>> MoveTestAnswer(int questionId, int oldIndex, int newIndex)
        {
            var answers = await _answerRepository.GetTestAnswers(a => a.QuestionId == questionId);
            var movedAnswer = answers.ElementAt(oldIndex);

            var oldPrevious = answers.ElementAtOrDefault(oldIndex - 1);
            var oldNext = answers.ElementAtOrDefault(oldIndex + 1);
            TestAnswerEntity newPrevious;
            TestAnswerEntity newNext;

            if (oldIndex < newIndex)
            {
                newPrevious = answers.ElementAtOrDefault(newIndex);
                newNext = answers.ElementAtOrDefault(newIndex + 1);
            }
            else
            {
                newPrevious = answers.ElementAtOrDefault(newIndex - 1);
                newNext = answers.ElementAtOrDefault(newIndex);
            }

            await HandleMoveTestAnswer(movedAnswer, newPrevious?.Id, newNext?.Id);
            await HandleMoveTestAnswer(oldPrevious, oldPrevious?.PreviousAnswerId, oldNext?.Id);
            await HandleMoveTestAnswer(oldNext, oldPrevious?.Id, oldNext?.NextAnswerId);
            await HandleMoveTestAnswer(newPrevious, newPrevious?.PreviousAnswerId, movedAnswer.Id);
            await HandleMoveTestAnswer(newNext, movedAnswer.Id, newNext?.NextAnswerId);

            return await GetTestAnswers(questionId);
        }

        private async Task HandleMoveTestAnswer(TestAnswerEntity answer, int? previousAnswerId, int? nextAnswerId)
        {
            if (answer != null)
            {
                answer.PreviousAnswerId = previousAnswerId;
                answer.NextAnswerId = nextAnswerId;
                await _answerRepository.UpdateTestAnswer(answer);
            }
        }

        private async Task HandleCorrectTestAnswer(TestAnswerEntity answer, string questionType)
        {
            switch (questionType)
            {
                case TestQuestionTypes.FreeAnswerQuestion:
                    answer.IsCorrect = true;
                    await _answerRepository.UpdateTestAnswer(answer);
                    break;
                case TestQuestionTypes.SingleAnswerQuestion:
                    answer.IsCorrect = true;
                    await _answerRepository.UpdateTestAnswer(answer);
                    break;
                case TestQuestionTypes.MultiAnswerQuestion:
                    break;
                default:
                    break;
            }
        }
    }
}
