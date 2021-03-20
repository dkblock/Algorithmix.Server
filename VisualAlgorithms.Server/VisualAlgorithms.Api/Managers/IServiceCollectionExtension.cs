using Microsoft.Extensions.DependencyInjection;

namespace VisualAlgorithms.Api.Managers
{
    public static class IServiceCollectionExtension
    {
        public static void AddManagers(this IServiceCollection services)
        {
            services.AddScoped<TestAnswersManager, TestAnswersManager>();
            services.AddScoped<TestQuestionsManager, TestQuestionsManager>();
            services.AddScoped<TestsManager, TestsManager>();
        }
    }
}
