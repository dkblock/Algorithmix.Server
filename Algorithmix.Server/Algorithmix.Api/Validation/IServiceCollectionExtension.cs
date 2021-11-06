using Microsoft.Extensions.DependencyInjection;

namespace Algorithmix.Api.Validation
{
    public static class IServiceCollectionExtension
    {
        public static void AddValidators(this IServiceCollection services)
        {
            services.AddScoped<AccountValidator, AccountValidator>();
            services.AddScoped<AlgorithmValidator, AlgorithmValidator>();
            services.AddScoped<GroupValidator, GroupValidator>();
            services.AddScoped<TestValidator, TestValidator>();
            services.AddScoped<TestAnswerValidator, TestAnswerValidator>();
            services.AddScoped<TestQuestionValidator, TestQuestionValidator>();
            services.AddScoped<TestPublishValidator, TestPublishValidator>();
        }
    }
}
