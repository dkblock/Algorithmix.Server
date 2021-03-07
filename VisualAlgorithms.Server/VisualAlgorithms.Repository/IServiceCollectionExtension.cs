using Microsoft.Extensions.DependencyInjection;

namespace VisualAlgorithms.Repository
{
    public static class IServiceCollectionExtension
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<AlgorithmsRepository, AlgorithmsRepository>();
            services.AddScoped<AlgorithmTimeComplexitiesRepository, AlgorithmTimeComplexitiesRepository>();
            services.AddScoped<TestsRepository, TestsRepository>();
            services.AddScoped<TestAnswersRepository, TestAnswersRepository>();
            services.AddScoped<TestQuestionsRepository, TestQuestionsRepository>();
        }
    }
}
