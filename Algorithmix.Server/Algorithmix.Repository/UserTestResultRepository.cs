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
    public class UserTestResultRepository
    {
        private readonly ApplicationContext _context;

        public UserTestResultRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<UserTestResultEntity> CreateUserTestResult(UserTestResultEntity userTestResult)
        {
            var result = await _context.UserTestResults.AddAsync(userTestResult);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<IEnumerable<UserTestResultEntity>> GetAllUserTestResults()
        {
            return await _context.UserTestResults.ToListAsync();
        }

        public async Task<IEnumerable<UserTestResultEntity>> GetUserTestResults(Expression<Func<UserTestResultEntity, bool>> predicate)
        {
            return await _context.UserTestResults.Where(predicate).ToListAsync();
        }

        public async Task<UserTestResultEntity> GetUserTestResultById(int testId, string userId)
        {
            return await _context.UserTestResults.FindAsync(testId, userId);
        }

        public async Task DeleteUserTestResult(int testId, string userId)
        {
            var userTestResult = await GetUserTestResultById(testId, userId);
            _context.UserTestResults.Remove(userTestResult);
            await _context.SaveChangesAsync();
        }

        public async Task<UserTestResultEntity> UpdateUserTestResult(UserTestResultEntity userTestResult)
        {
            var userTestResultToUpdate = await GetUserTestResultById(userTestResult.TestId, userTestResult.UserId);
            _context.Entry(userTestResultToUpdate).CurrentValues.SetValues(userTestResult);
            await _context.SaveChangesAsync();

            return await GetUserTestResultById(userTestResult.TestId, userTestResult.UserId);
        }
    }
}
