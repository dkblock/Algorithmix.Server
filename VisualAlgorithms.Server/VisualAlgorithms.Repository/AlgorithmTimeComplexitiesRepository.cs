using System.Threading.Tasks;
using VisualAlgorithms.Database;
using VisualAlgorithms.Entities;

namespace VisualAlgorithms.Repository
{
    public class AlgorithmTimeComplexitiesRepository
    {
        private readonly ApplicationContext _context;

        public AlgorithmTimeComplexitiesRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<AlgorithmTimeComplexityEntity> GetAlgorithmTimeComplexityById(int id)
        {
            return await _context.AlgorithmTimeComplexities.FindAsync(id);
        }
    }
}
