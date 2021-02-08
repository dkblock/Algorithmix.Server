using Microsoft.Extensions.DependencyInjection;

namespace VisualAlgorithms.Repository
{
    public static class IServiceCollectionExtension
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<TestsRepository, TestsRepository>();
            services.AddSingleton<TestAnswersRepository, TestAnswersRepository>();
            services.AddSingleton<TestQuestionsRepository, TestQuestionsRepository>();
        }
    }
}
