using Microsoft.Extensions.DependencyInjection;

namespace VisualAlgorithms.Mappers
{
    public static class IServiceCollectionExtension
    {
        public static void AddMappers(this IServiceCollection services)
        {
            services.AddScoped<AlgorithmsMapper, AlgorithmsMapper>();
            services.AddScoped<AlgorithmTimeComplexitiesMapper, AlgorithmTimeComplexitiesMapper>();
            services.AddScoped<TestsMapper, TestsMapper>();
            services.AddScoped<TestAnswersMapper, TestAnswersMapper>();
            services.AddScoped<TestQuestionsMapper, TestQuestionsMapper>();
        }
    }
}
