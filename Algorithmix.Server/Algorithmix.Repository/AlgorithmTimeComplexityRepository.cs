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

        public async Task<IEnumerable<AlgorithmTimeComplexityEntity>> GetAllAlgorithmTimeComplexities()
        {
            return await _context.AlgorithmTimeComplexities.ToListAsync();
        }

        public async Task<AlgorithmTimeComplexityEntity> GetAlgorithmTimeComplexityById(int id)
        {
            return await _context.AlgorithmTimeComplexities.FindAsync(id);
        }
    }
}
