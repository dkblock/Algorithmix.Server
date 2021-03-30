using Microsoft.Extensions.DependencyInjection;

namespace Algorithmix.Api.Managers
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
