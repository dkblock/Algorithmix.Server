using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Algorithmix.Database;
using Algorithmix.Entities;

namespace Algorithmix.Repository
{
    public class TestAnswerRepository
    {
        private readonly ApplicationContext _context;

        public TestAnswerRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<TestAnswerEntity> CreateTestAnswer(TestAnswerEntity answer)
        {
            var result = await _context.TestAnswers.AddAsync(answer);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<IEnumerable<TestAnswerEntity>> GetAllTestAnswers()
        {
            return await _context.TestAnswers.ToListAsync();
        }

        public async Task<IEnumerable<TestAnswerEntity>> GetTestAnswers(Expression<Func<TestAnswerEntity, bool>> predicate)
        {
            return await _context.TestAnswers.Where(predicate).ToListAsync();
        }

        public async Task<TestAnswerEntity> GetTestAnswerById(int id)
        {
            return await _context.TestAnswers.FindAsync(id);
        }

        public async Task DeleteTestAnswer(int id)
        {
            var answer = await GetTestAnswerById(id);
            _context.TestAnswers.Remove(answer);
            await _context.SaveChangesAsync();
        }

        public async Task<TestAnswerEntity> UpdateTestAnswer(TestAnswerEntity answer)
        {
            var answerToUpdate = await GetTestAnswerById(answer.Id);
            _context.Entry(answerToUpdate).CurrentValues.SetValues(answer);
            await _context.SaveChangesAsync();

            return await GetTestAnswerById(answer.Id);
        }
    }
}
