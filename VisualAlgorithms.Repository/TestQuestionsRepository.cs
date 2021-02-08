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
    public class TestQuestionsRepository
    {
        private readonly ApplicationContext _context;

        public TestQuestionsRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<int> AddTestQuestion(TestQuestionEntity testQuestion)
        {
            var result = await _context.TestQuestions.AddAsync(testQuestion);
            await _context.SaveChangesAsync();

            return result.Entity.Id;
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
    }
}
