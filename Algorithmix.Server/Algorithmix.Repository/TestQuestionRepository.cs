using Algorithmix.Database;
using Algorithmix.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Algorithmix.Repository
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
            return await _context.TestQuestions.Where(predicate).ToListAsync();
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
