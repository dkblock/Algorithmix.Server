using Microsoft.Extensions.DependencyInjection;

namespace VisualAlgorithms.Api.Managers
{
    public static class IServiceCollectionExtension
    {
        public static void AddManagers(this IServiceCollection services)
        {
            services.AddScoped<TestAnswerManager, TestAnswerManager>();
            services.AddScoped<TestQuestionManager, TestQuestionManager>();
            services.AddScoped<TestManager, TestManager>();
        }
    }
}
