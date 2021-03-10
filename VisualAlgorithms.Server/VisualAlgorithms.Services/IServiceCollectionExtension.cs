using Microsoft.Extensions.DependencyInjection;

namespace VisualAlgorithms.Services
{
    public static class IServiceCollectionExtension
    {
        public static void AddCommonServices(this IServiceCollection services)
        {
            services.AddScoped<AccountService, AccountService>();
            services.AddScoped<AlgorithmsService, AlgorithmsService>();
            services.AddScoped<GroupsService, GroupsService>();
            services.AddScoped<TestsService, TestsService>();
            services.AddScoped<TestAnswersService, TestAnswersService>();
            services.AddScoped<TestQuestionsService, TestQuestionsService>();
            services.AddScoped<UsersService, UsersService>();
        }
    }
}
