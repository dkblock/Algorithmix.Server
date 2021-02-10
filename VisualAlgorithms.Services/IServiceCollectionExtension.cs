using Microsoft.Extensions.DependencyInjection;

namespace VisualAlgorithms.Services
{
    public static class IServiceCollectionExtension
    {
        public static void AddCommonServices(this IServiceCollection services)
        {
            services.AddSingleton<TestsService, TestsService>();
            services.AddSingleton<TestAnswersService, TestAnswersService>();
            services.AddSingleton<TestQuestionsService, TestQuestionsService>();
        }
    }
}
