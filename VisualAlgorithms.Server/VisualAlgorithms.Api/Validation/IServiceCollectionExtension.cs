using Microsoft.Extensions.DependencyInjection;

namespace VisualAlgorithms.Api.Validation
{
    public static class IServiceCollectionExtension
    {
        public static void AddValidators(this IServiceCollection services)
        {
            services.AddScoped<AccountValidator, AccountValidator>();
            services.AddScoped<TestValidator, TestValidator>();
            services.AddScoped<TestAnswerValidator, TestAnswerValidator>();
            services.AddScoped<TestQuestionValidator, TestQuestionValidator>();
        }
    }
}
