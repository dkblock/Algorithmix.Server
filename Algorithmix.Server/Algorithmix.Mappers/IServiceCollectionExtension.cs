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
            services.AddScoped<GroupMapper, GroupMapper>();
            services.AddScoped<TestMapper, TestMapper>();
            services.AddScoped<TestAlgorithmMapper, TestAlgorithmMapper>();
            services.AddScoped<TestAnswerMapper, TestAnswerMapper>();
            services.AddScoped<TestQuestionMapper, TestQuestionMapper>();
            services.AddScoped<UserAnswerMapper, UserAnswerMapper>();
            services.AddScoped<UserTestResultMapper, UserTestResultMapper>();
        }
    }
}
