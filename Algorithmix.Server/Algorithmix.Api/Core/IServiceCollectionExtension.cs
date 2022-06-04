using Algorithmix.Api.Core.Helpers;
using Algorithmix.Api.Core.TestDesign;
using Algorithmix.Api.Core.TestPass;
using Microsoft.Extensions.DependencyInjection;

namespace Algorithmix.Api.Core
{
    public static class IServiceCollectionExtension
    {
        public static void AddManagers(this IServiceCollection services)
        {
            services.AddScoped<AccountManager, AccountManager>();
            services.AddScoped<IAlgorithmDataManager, AlgorithmDataManager>();
            services.AddScoped<AlgorithmManager, AlgorithmManager>();
            services.AddScoped<ApplicationUserManager, ApplicationUserManager>();
            services.AddScoped<MailManager, MailManager>();
            services.AddScoped<IFileManager, FileManager>();
            services.AddScoped<GroupManager, GroupManager>();
            services.AddScoped<PublishedTestAnswerManager, PublishedTestAnswerManager>();
            services.AddScoped<PublishedTestQuestionManager, PublishedTestQuestionManager>();
            services.AddScoped<PublishedTestManager, PublishedTestManager>();
            services.AddScoped<TestDataManager, TestDataManager>();
            services.AddScoped<TestAnswerManager, TestAnswerManager>();
            services.AddScoped<TestQuestionManager, TestQuestionManager>();
            services.AddScoped<TestManager, TestManager>();
            services.AddScoped<TestPassManager, TestPassManager>();
            services.AddScoped<TestPublishManager, TestPublishManager>();
            services.AddScoped<ITestStatsManager, TestStatsManager>();
            services.AddScoped<UserAnswerManager, UserAnswerManager>();
            services.AddScoped<UserTestResultManager, UserTestResultManager>();
            services.AddScoped<QueryHelper, QueryHelper>();
        }
    }
}
