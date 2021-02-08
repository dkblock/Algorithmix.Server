using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VisualAlgorithms.Database;
using VisualAlgorithms.Entities;

namespace VisualAlgorithms.Repository
{
    public class TestAnswersRepository
    {
        private readonly ApplicationContext _context;

        public TestAnswersRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<int> AddTestQuestion(TestAnswerEntity testAnswer)
        {
            var result = await _context.TestAnswers.AddAsync(testAnswer);
            await _context.SaveChangesAsync();

            return result.Entity.Id;
        }

        public async Task<IEnumerable<TestAnswerEntity>> GetAllTestAnswers()
        {
            return await _context.TestAnswers.ToListAsync();
        }

        public async Task<IEnumerable<TestAnswerEntity>> GetTestAnswers(Expression<Func<TestAnswerEntity, bool>> predicate)
        {
            return await _context.TestAnswers.Where(predicate).ToListAsync();
        }

        public async Task<TestAnswerEntity> GetTestById(int id)
        {
            return await _context.TestAnswers.FindAsync(id);
        }
    }
}
