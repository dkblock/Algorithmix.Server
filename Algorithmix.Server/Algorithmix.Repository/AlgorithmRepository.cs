using Algorithmix.Database;
using Algorithmix.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Algorithmix.Repository
{
    public class AlgorithmRepository
    {
        private readonly ApplicationContext _context;

        public AlgorithmRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<AlgorithmEntity> CreateAlgorithm(AlgorithmEntity algorithm)
        {
            var result = await _context.Algorithms.AddAsync(algorithm);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<IEnumerable<AlgorithmEntity>> GetAllAlgorithms()
        {
            return await _context.Algorithms.ToListAsync();
        }

        public async Task<IEnumerable<AlgorithmEntity>> GetAlgorithms(Expression<Func<AlgorithmEntity, bool>> predicate)
        {
            return await _context.Algorithms.Where(predicate).ToListAsync();
        }

        public async Task<AlgorithmEntity> GetAlgorithmById(string id)
        {
            return await _context.Algorithms.FindAsync(id);
        }

        public async Task DeleteAlgorithm(string id)
        {
            var algorithm = await GetAlgorithmById(id);
            _context.Algorithms.Remove(algorithm);
            await _context.SaveChangesAsync();
        }

        public async Task<AlgorithmEntity> UpdateAlgorithm(AlgorithmEntity algorithm)
        {
            var algorithmToUpdate = await GetAlgorithmById(algorithm.Id);
            _context.Entry(algorithmToUpdate).CurrentValues.SetValues(algorithm);
            await _context.SaveChangesAsync();

            return await GetAlgorithmById(algorithm.Id);
        }
    }
}
