using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Algorithmix.Database;
using Algorithmix.Entities;

namespace Algorithmix.Repository
{
    public class AlgorithmRepository
    {
        private readonly ApplicationContext _context;

        public AlgorithmRepository(ApplicationContext context)
        {
            _context = context;
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
    }
}
