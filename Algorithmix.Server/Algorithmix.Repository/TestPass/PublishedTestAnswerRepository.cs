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
    public class PublishedTestAnswerRepository
    {
        private readonly ApplicationContext _context;

        public PublishedTestAnswerRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<PublishedTestAnswerEntity> CreateTestAnswer(PublishedTestAnswerEntity answer)
        {
            var result = await _context.PublishedTestAnswers.AddAsync(answer);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<IEnumerable<PublishedTestAnswerEntity>> GetAllTestAnswers()
        {
            return await _context.PublishedTestAnswers.ToListAsync();
        }

        public async Task<IEnumerable<PublishedTestAnswerEntity>> GetTestAnswers(Expression<Func<PublishedTestAnswerEntity, bool>> predicate)
        {
            var answers = await _context.PublishedTestAnswers.Where(predicate).ToListAsync();
            return GetOrderedTestAnswers(answers);
        }

        private IEnumerable<PublishedTestAnswerEntity> GetOrderedTestAnswers(IEnumerable<PublishedTestAnswerEntity> answers)
        {
            var orderedAnswers = new List<PublishedTestAnswerEntity>();
            var next = answers.SingleOrDefault(q => q.PreviousAnswerId == null);

            while (next != null)
            {
                orderedAnswers.Add(next);
                next = answers.SingleOrDefault(q => q.Id == next.NextAnswerId);

                if (orderedAnswers.Count > 100)           // TODO: Need to investigate problem when there is infinite count of answers
                    break;
            }

            return orderedAnswers;
        }

        public async Task<PublishedTestAnswerEntity> GetTestAnswerById(int id)
        {
            return await _context.PublishedTestAnswers.FindAsync(id);
        }

        public async Task DeleteTestAnswer(int id)
        {
            var answer = await GetTestAnswerById(id);
            _context.PublishedTestAnswers.Remove(answer);
            await _context.SaveChangesAsync();
        }

        public async Task<PublishedTestAnswerEntity> UpdateTestAnswer(PublishedTestAnswerEntity answer)
        {
            var answerToUpdate = await GetTestAnswerById(answer.Id);
            _context.Entry(answerToUpdate).CurrentValues.SetValues(answer);
            await _context.SaveChangesAsync();

            return await GetTestAnswerById(answer.Id);
        }
    }
}
