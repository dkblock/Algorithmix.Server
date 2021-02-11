using Microsoft.Extensions.DependencyInjection;

namespace VisualAlgorithms.Services
{
    public static class IServiceCollectionExtension
    {
        public static void AddCommonServices(this IServiceCollection services)
        {
            services.AddScoped<AlgorithmsService, AlgorithmsService>();
            services.AddScoped<TestsService, TestsService>();
            services.AddScoped<TestAnswersService, TestAnswersService>();
            services.AddScoped<TestQuestionsService, TestQuestionsService>();
        }
    }
}
