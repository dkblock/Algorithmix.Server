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
    public class TestRepository
    {
        private readonly ApplicationContext _context;

        public TestRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<TestEntity> CreateTest(TestEntity test)
        {
            var result = await _context.Tests.AddAsync(test);
            await _context.SaveChangesAsync();

            return result.Entity;
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

        public async Task DeleteTest(int id)
        {
            var test = await GetTestById(id);
            _context.Tests.Remove(test);
            await _context.SaveChangesAsync();
        }

        public async Task<TestEntity> UpdateTest(TestEntity test)
        {
            var testToUpdate = await GetTestById(test.Id);
            _context.Entry(testToUpdate).CurrentValues.SetValues(test);
            await _context.SaveChangesAsync();

            return await GetTestById(test.Id);
        }
    }
}
