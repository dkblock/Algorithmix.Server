using Microsoft.Extensions.DependencyInjection;

namespace Algorithmix.Mappers
{
    public static class IServiceCollectionExtension
    {
        public static void AddMappers(this IServiceCollection services)
        {
            services.AddScoped<AlgorithmMapper, AlgorithmMapper>();
            services.AddScoped<AlgorithmTimeComplexityMapper, AlgorithmTimeComplexityMapper>();
            services.AddScoped<ApplicationUserMapper, ApplicationUserMapper>();
            services.AddScoped<TestMapper, TestMapper>();
            services.AddScoped<TestAnswerMapper, TestAnswerMapper>();
            services.AddScoped<TestQuestionMapper, TestQuestionMapper>();
        }
    }
}
