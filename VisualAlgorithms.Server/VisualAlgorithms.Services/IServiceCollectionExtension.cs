using Microsoft.Extensions.DependencyInjection;

namespace VisualAlgorithms.Services
{
    public static class IServiceCollectionExtension
    {
        public static void AddCommonServices(this IServiceCollection services)
        {
            services.AddScoped<AccountService, AccountService>();
            services.AddScoped<AlgorithmService, AlgorithmService>();
            services.AddScoped<GroupsService, GroupsService>();
            services.AddScoped<TestService, TestService>();
            services.AddScoped<TestAnswerService, TestAnswerService>();
            services.AddScoped<TestQuestionService, TestQuestionService>();
            services.AddScoped<UserService, UserService>();
        }
    }
}
