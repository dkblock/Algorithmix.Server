using Algorithmix.Services.TestDesign;
using Algorithmix.Services.TestPass;
using Microsoft.Extensions.DependencyInjection;

namespace Algorithmix.Services
{
    public static class IServiceCollectionExtension
    {
        public static void AddCommonServices(this IServiceCollection services)
        {
            services.AddScoped<AccountService, AccountService>();
            services.AddScoped<AlgorithmService, AlgorithmService>();
            services.AddScoped<GroupService, GroupService>();
            services.AddScoped<PublishedTestService, PublishedTestService>();
            services.AddScoped<PublishedTestAnswerService, PublishedTestAnswerService>();
            services.AddScoped<PublishedTestQuestionService, PublishedTestQuestionService>();
            services.AddScoped<TestService, TestService>();
            services.AddScoped<TestAnswerService, TestAnswerService>();
            services.AddScoped<TestQuestionService, TestQuestionService>();
            services.AddScoped<UserAnswerService, UserAnswerService>();
            services.AddScoped<UserTestResultService, UserTestResultService>();
            services.AddScoped<UserService, UserService>();
        }
    }
}
