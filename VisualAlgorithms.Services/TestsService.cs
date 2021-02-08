using System.Collections.Generic;
using VisualAlgorithms.Mappers;

namespace VisualAlgorithms.Services
{
    public class TestsService
    {
        private readonly TestsMapper _testsMapper;

        public TestsService(TestsMapper testsMapper)
        {
            _testsMapper = testsMapper;
        }
    }
}
