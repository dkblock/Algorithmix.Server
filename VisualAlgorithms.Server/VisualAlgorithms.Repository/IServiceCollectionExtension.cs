using Microsoft.Extensions.DependencyInjection;

namespace VisualAlgorithms.Repository
{
    public static class IServiceCollectionExtension
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<AlgorithmRepository, AlgorithmRepository>();
            services.AddScoped<AlgorithmTimeComplexityRepository, AlgorithmTimeComplexityRepository>();
            services.AddScoped<GroupRepository, GroupRepository>();
            services.AddScoped<TestRepository, TestRepository>();
            services.AddScoped<TestAnswerRepository, TestAnswerRepository>();
            services.AddScoped<TestQuestionRepository, TestQuestionRepository>();
        }
    }
}
