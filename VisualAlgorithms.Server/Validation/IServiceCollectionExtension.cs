using Microsoft.Extensions.DependencyInjection;

namespace VisualAlgorithms.Server.Validation
{
    public static class IServiceCollectionExtension
    {
        public static void AddValidators(this IServiceCollection services)
        {
            services.AddScoped<AccountValidator, AccountValidator>();
            services.AddScoped<TestsValidator, TestsValidator>();
            services.AddScoped<TestAnswersValidator, TestAnswersValidator>();
            services.AddScoped<TestQuestionsValidator, TestQuestionsValidator>();
        }
    }
}
