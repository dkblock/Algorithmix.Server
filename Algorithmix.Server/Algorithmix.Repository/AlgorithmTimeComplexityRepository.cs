using Algorithmix.Database;
using Algorithmix.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Algorithmix.Repository
{
    public class AlgorithmTimeComplexityRepository
    {
        private readonly ApplicationContext _context;

        public AlgorithmTimeComplexityRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<AlgorithmTimeComplexityEntity> CreateAlgorithmTimeComplexity(AlgorithmTimeComplexityEntity algorithmTimeComplexity)
        {
            var result = await _context.AlgorithmTimeComplexities.AddAsync(algorithmTimeComplexity);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<IEnumerable<AlgorithmTimeComplexityEntity>> GetAllAlgorithmTimeComplexities()
        {
            return await _context.AlgorithmTimeComplexities.ToListAsync();
        }

        public async Task<AlgorithmTimeComplexityEntity> GetAlgorithmTimeComplexityById(int id)
        {
            return await _context.AlgorithmTimeComplexities.FindAsync(id);
        }

        public async Task DeleteAlgorithmTimeComplexity(int id)
        {
            var algorithmTimeComplexity = await GetAlgorithmTimeComplexityById(id);
            _context.AlgorithmTimeComplexities.Remove(algorithmTimeComplexity);
            await _context.SaveChangesAsync();
        }

        public async Task<AlgorithmTimeComplexityEntity> UpdateAlgorithmTimeComplexity(AlgorithmTimeComplexityEntity algorithmTimeComplexity)
        {
            var timeComplexityToUpdate = await GetAlgorithmTimeComplexityById(algorithmTimeComplexity.Id);
            _context.Entry(timeComplexityToUpdate).CurrentValues.SetValues(algorithmTimeComplexity);
            await _context.SaveChangesAsync();

            return await GetAlgorithmTimeComplexityById(algorithmTimeComplexity.Id);
        }
    }
}
