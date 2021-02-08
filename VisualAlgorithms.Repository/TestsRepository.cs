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
    public class TestsRepository
    {
        private readonly ApplicationContext _context;

        public TestsRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<int> AddTest(TestEntity test)
        {
            var result = await _context.Tests.AddAsync(test);
            await _context.SaveChangesAsync();

            return result.Entity.Id;
        }

        public async Task<IEnumerable<TestEntity>> GetAllTests()
        {
            return await _context.Tests.ToListAsync();
        }

        public async Task<IEnumerable<TestEntity>> GetTests(Expression<Func<TestEntity, bool>> predicate)
        {
            return await _context.Tests.Where(predicate).ToListAsync();
        }

        public async Task<TestEntity> GetTestById(int id)
        {
            return await _context.Tests.FindAsync(id);
        }
    }
}
