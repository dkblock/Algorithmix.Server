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
    public class UserAnswerRepository
    {
        private readonly ApplicationContext _context;

        public UserAnswerRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<UserAnswerEntity> CreateUserAnswer(UserAnswerEntity userAnswer)
        {
            var result = await _context.UserAnswers.AddAsync(userAnswer);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<IEnumerable<UserAnswerEntity>> GetAllUserAnswers()
        {
            return await _context.UserAnswers.ToListAsync();
        }

        public async Task<IEnumerable<UserAnswerEntity>> GetUserAnswers(Expression<Func<UserAnswerEntity, bool>> predicate)
        {
            return await _context.UserAnswers.Where(predicate).ToListAsync();
        }

        public async Task<UserAnswerEntity> GetUserAnswerById(int questionId, string userId)
        {
            return await _context.UserAnswers.FindAsync(questionId, userId);
        }

        public async Task DeleteUserAnswer(int questionId, string userId)
        {
            var userAnswer = await GetUserAnswerById(questionId, userId);
            _context.UserAnswers.Remove(userAnswer);
            await _context.SaveChangesAsync();
        }

        public async Task<UserAnswerEntity> UpdateUserAnswer(UserAnswerEntity userAnswer)
        {
            var userAnswerToUpdate = await GetUserAnswerById(userAnswer.QuestionId, userAnswer.UserId);
            _context.Entry(userAnswerToUpdate).CurrentValues.SetValues(userAnswer);
            await _context.SaveChangesAsync();

            return await GetUserAnswerById(userAnswer.QuestionId, userAnswer.UserId);
        }
    }
}
