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
    public class PublishedTestQuestionRepository
    {
        private readonly ApplicationContext _context;

        public PublishedTestQuestionRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<PublishedTestQuestionEntity> CreateTestQuestion(PublishedTestQuestionEntity testQuestion)
        {
            var result = await _context.PublishedTestQuestions.AddAsync(testQuestion);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<IEnumerable<PublishedTestQuestionEntity>> GetAllTestQuestions()
        {
            return await _context.PublishedTestQuestions.ToListAsync();
        }

        public async Task<IEnumerable<PublishedTestQuestionEntity>> GetTestQuestions(Expression<Func<PublishedTestQuestionEntity, bool>> predicate)
        {
            var questions = await _context.PublishedTestQuestions.Where(predicate).ToListAsync();
            return GetOrderedTestQuestions(questions);
        }

        private IEnumerable<PublishedTestQuestionEntity> GetOrderedTestQuestions(IEnumerable<PublishedTestQuestionEntity> questions)
        {
            var orderedQuestions = new List<PublishedTestQuestionEntity>();
            var next = questions.SingleOrDefault(q => q.PreviousQuestionId == null);

            while (next != null)
            {
                orderedQuestions.Add(next);
                next = questions.SingleOrDefault(q => q.Id == next.NextQuestionId);

                if (orderedQuestions.Count > 100)           // TODO: Need to investigate problem when there is infinite count of questions
                    break;
            }

            return orderedQuestions;
        }

        public async Task<PublishedTestQuestionEntity> GetTestQuestionById(int id)
        {
            return await _context.PublishedTestQuestions.FindAsync(id);
        }

        public async Task DeleteTestQuestion(int id)
        {
            var question = await GetTestQuestionById(id);
            _context.PublishedTestQuestions.Remove(question);
            await _context.SaveChangesAsync();
        }

        public async Task<PublishedTestQuestionEntity> UpdateTestQuestion(PublishedTestQuestionEntity question)
        {
            var questionToUpdate = await GetTestQuestionById(question.Id);
            _context.Entry(questionToUpdate).CurrentValues.SetValues(question);
            await _context.SaveChangesAsync();

            return await GetTestQuestionById(question.Id);
        }
    }
}
