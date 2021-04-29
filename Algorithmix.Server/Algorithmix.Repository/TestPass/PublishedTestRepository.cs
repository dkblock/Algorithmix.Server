using Algorithmix.Database;
using Algorithmix.Entities.Test;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Algorithmix.Repository.TestPass
{
    public class PublishedTestRepository
    {
        private readonly ApplicationContext _context;

        public PublishedTestRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<PublishedTestEntity> CreateTest(PublishedTestEntity test)
        {
            var result = await _context.PublishedTests.AddAsync(test);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<IEnumerable<PublishedTestEntity>> GetAllTests()
        {
            return await _context.PublishedTests.ToListAsync();
        }

        public async Task<IEnumerable<PublishedTestEntity>> GetTests(Expression<Func<PublishedTestEntity, bool>> predicate)
        {
            return await _context.PublishedTests.Where(predicate).ToListAsync();
        }

        public async Task<PublishedTestEntity> GetTestById(int id)
        {
            return await _context.PublishedTests.FindAsync(id);
        }

        public async Task DeleteTest(int id)
        {
            var test = await GetTestById(id);
            _context.PublishedTests.Remove(test);
            await _context.SaveChangesAsync();
        }

        public async Task<PublishedTestEntity> UpdateTest(PublishedTestEntity test)
        {
            var testToUpdate = await GetTestById(test.Id);
            _context.Entry(testToUpdate).CurrentValues.SetValues(test);
            await _context.SaveChangesAsync();

            return await GetTestById(test.Id);
        }
    }
}
