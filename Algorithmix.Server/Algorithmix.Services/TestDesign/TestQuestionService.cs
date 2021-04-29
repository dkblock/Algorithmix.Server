using Algorithmix.Entities.Test;
using Algorithmix.Mappers;
using Algorithmix.Models.Tests;
using Algorithmix.Repository.TestDesign;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Algorithmix.Services.TestDesign
{
    public class TestQuestionService
    {
        private readonly TestQuestionMapper _questionMapper;
        private readonly TestQuestionRepository _questionRepository;

        public TestQuestionService(TestQuestionMapper questionMapper, TestQuestionRepository questionRepository)
        {
            _questionMapper = questionMapper;
            _questionRepository = questionRepository;
        }

        public async Task<TestQuestion> CreateTestQuestion(TestQuestionPayload questionPayload)
        {
            var questions = await _questionRepository.GetTestQuestions(q => q.TestId == questionPayload.TestId);
            var questionEntity = _questionMapper.ToEntity(questionPayload);
            var createdQuestion = await _questionRepository.CreateTestQuestion(questionEntity);

            if (questions.Any())
            {
                var lastQuestion = questions.Last();
                createdQuestion.PreviousQuestionId = lastQuestion.Id;
                lastQuestion.NextQuestionId = createdQuestion.Id;

                await _questionRepository.UpdateTestQuestion(lastQuestion);
                await _questionRepository.UpdateTestQuestion(createdQuestion);
            }

            return _questionMapper.ToModel(createdQuestion);
        }

        public async Task<bool> Exists(int questionId, int testId)
        {
            var questionEntity = await _questionRepository.GetTestQuestionById(questionId);
            return questionEntity != null && questionEntity.TestId == testId;
        }

        public async Task<TestQuestion> GetTestQuestion(int id)
        {
            var questionEntity = await _questionRepository.GetTestQuestionById(id);
            return _questionMapper.ToModel(questionEntity);
        }

        public async Task<IEnumerable<TestQuestion>> GetTestQuestions(int testId)
        {
            var questionEntities = await _questionRepository.GetTestQuestions(q => q.TestId == testId);
            return _questionMapper.ToModelCollection(questionEntities);
        }

        public async Task<IEnumerable<TestQuestion>> GetTestQuestions(IEnumerable<int> testIds)
        {
            var questionEntities = await _questionRepository.GetTestQuestions(q => testIds.Contains(q.TestId));
            return _questionMapper.ToModelCollection(questionEntities);
        }

        public async Task DeleteTestQuestion(int id)
        {
            var question = await _questionRepository.GetTestQuestionById(id);

            if (question.PreviousQuestionId != null)
            {
                var previousQuestion = await _questionRepository.GetTestQuestionById((int)question.PreviousQuestionId);
                previousQuestion.NextQuestionId = question.NextQuestionId;
                await _questionRepository.UpdateTestQuestion(previousQuestion);
            }

            if (question.NextQuestionId != null)
            {
                var nextQuestion = await _questionRepository.GetTestQuestionById((int)question.NextQuestionId);
                nextQuestion.PreviousQuestionId = question.PreviousQuestionId;
                await _questionRepository.UpdateTestQuestion(nextQuestion);
            }

            await _questionRepository.DeleteTestQuestion(id);
        }

        public async Task<TestQuestion> UpdateTestQuestion(int id, TestQuestionPayload questionPayload)
        {
            var questionEntity = _questionMapper.ToEntity(questionPayload, id);
            var updatedQuestion = await _questionRepository.UpdateTestQuestion(questionEntity);

            return _questionMapper.ToModel(updatedQuestion);
        }

        public async Task<TestQuestion> UpdateTestQuestionImage(int id, string imagePath)
        {
            var questionEntity = await _questionRepository.GetTestQuestionById(id);
            questionEntity.Image = imagePath;
            var updatedQuestion = await _questionRepository.UpdateTestQuestion(questionEntity);

            return _questionMapper.ToModel(updatedQuestion);
        }

        public async Task<string> ClearTestQuestionImage(int id)
        {
            var questionEntity = await _questionRepository.GetTestQuestionById(id);
            var imagePath = questionEntity.Image;

            questionEntity.Image = null;
            await _questionRepository.UpdateTestQuestion(questionEntity);

            return imagePath;
        }

        public async Task<IEnumerable<TestQuestion>> MoveTestQuestion(int testId, int oldIndex, int newIndex)
        {
            var questions = await _questionRepository.GetTestQuestions(q => q.TestId == testId);
            var movedQuestion = questions.ElementAt(oldIndex);

            var oldPrevious = questions.ElementAtOrDefault(oldIndex - 1);
            var oldNext = questions.ElementAtOrDefault(oldIndex + 1);
            TestQuestionEntity newPrevious;
            TestQuestionEntity newNext;

            if (oldIndex < newIndex)
            {
                newPrevious = questions.ElementAtOrDefault(newIndex);
                newNext = questions.ElementAtOrDefault(newIndex + 1);
            }
            else
            {
                newPrevious = questions.ElementAtOrDefault(newIndex - 1);
                newNext = questions.ElementAtOrDefault(newIndex);
            }

            await HandleMoveTestQuestion(movedQuestion, newPrevious?.Id, newNext?.Id);
            await HandleMoveTestQuestion(oldPrevious, oldPrevious?.PreviousQuestionId, oldNext?.Id);
            await HandleMoveTestQuestion(oldNext, oldPrevious?.Id, oldNext?.NextQuestionId);
            await HandleMoveTestQuestion(newPrevious, newPrevious?.PreviousQuestionId, movedQuestion.Id);
            await HandleMoveTestQuestion(newNext, movedQuestion.Id, newNext?.NextQuestionId);

            return await GetTestQuestions(testId);
        }

        private async Task HandleMoveTestQuestion(TestQuestionEntity question, int? previousQuestionId, int? nextQuestionId)
        {
            if (question != null)
            {
                question.PreviousQuestionId = previousQuestionId;
                question.NextQuestionId = nextQuestionId;
                await _questionRepository.UpdateTestQuestion(question);
            }
        }
    }
}
