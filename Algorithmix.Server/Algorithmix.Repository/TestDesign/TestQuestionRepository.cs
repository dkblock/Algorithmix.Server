using Algorithmix.Database;
using Algorithmix.Entities.Test;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Algorithmix.Repository.TestDesign
{
    public class TestQuestionRepository
    {
        private readonly ApplicationContext _context;

        public TestQuestionRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<TestQuestionEntity> CreateTestQuestion(TestQuestionEntity testQuestion)
        {
            var result = await _context.TestQuestions.AddAsync(testQuestion);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<IEnumerable<TestQuestionEntity>> GetAllTestQuestions()
        {
            return await _context.TestQuestions.ToListAsync();
        }

        public async Task<IEnumerable<TestQuestionEntity>> GetTestQuestions(Expression<Func<TestQuestionEntity, bool>> predicate)
        {
            var questions = await _context.TestQuestions.Where(predicate).ToListAsync();
            return GetOrderedTestQuestions(questions);
        }

        private IEnumerable<TestQuestionEntity> GetOrderedTestQuestions(IEnumerable<TestQuestionEntity> questions)
        {
            var orderedQuestions = new List<TestQuestionEntity>();
            var next = questions.SingleOrDefault(q => q.PreviousQuestionId == null);

            while (next != null)
            {
                orderedQuestions.Add(next);
                next = questions.SingleOrDefault(q => q.Id == next.NextQuestionId);

                if (orderedQuestions.Count > 100)           // TODO: Need to investigate problem when there is infinite count of questions
                    break;
            }

            return orderedQuestions;
        }

        public async Task<TestQuestionEntity> GetTestQuestionById(int id)
        {
            return await _context.TestQuestions.FindAsync(id);
        }

        public async Task DeleteTestQuestion(int id)
        {
            var question = await GetTestQuestionById(id);
            _context.TestQuestions.Remove(question);
            await _context.SaveChangesAsync();
        }

        public async Task<TestQuestionEntity> UpdateTestQuestion(TestQuestionEntity question)
        {
            var questionToUpdate = await GetTestQuestionById(question.Id);
            _context.Entry(questionToUpdate).CurrentValues.SetValues(question);
            await _context.SaveChangesAsync();

            return await GetTestQuestionById(question.Id);
        }
    }
}
