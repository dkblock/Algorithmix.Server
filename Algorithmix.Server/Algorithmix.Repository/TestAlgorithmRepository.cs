using Algorithmix.Database;
using Algorithmix.Entities.Test;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Algorithmix.Repository
{
    public class TestAlgorithmRepository
    {
        private readonly ApplicationContext _context;

        public TestAlgorithmRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<TestAlgorithmEntity> CreateTestAlgorithm(TestAlgorithmEntity testAlgorithm)
        {
            var result = _context.TestAlgorithms.Add(testAlgorithm);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<IEnumerable<TestAlgorithmEntity>> GetAllTestAlgorithms()
        {
            return await _context.TestAlgorithms.ToListAsync();
        }

        public async Task<IEnumerable<TestAlgorithmEntity>> GetTestAlgorithms(Expression<Func<TestAlgorithmEntity, bool>> predicate)
        {
            return await _context.TestAlgorithms.Where(predicate).ToListAsync();
        }

        public async Task<TestAlgorithmEntity> GetTestAlgorithmById(int id)
        {
            return await _context.TestAlgorithms.FindAsync(id);
        }

        public async Task DeleteTestAlgorithm(int id)
        {
            var testAlgorithm = await GetTestAlgorithmById(id);
            _context.TestAlgorithms.Remove(testAlgorithm);
            await _context.SaveChangesAsync();
        }
    }
}
