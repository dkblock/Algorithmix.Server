﻿using Microsoft.Extensions.DependencyInjection;

namespace Algorithmix.Api.Core
{
    public static class IServiceCollectionExtension
    {
        public static void AddManagers(this IServiceCollection services)
        {
            services.AddScoped<TestDataManager, TestDataManager>();
            services.AddScoped<TestAnswerManager, TestAnswerManager>();
            services.AddScoped<TestQuestionManager, TestQuestionManager>();
            services.AddScoped<TestManager, TestManager>();
            services.AddScoped<TestPassManager, TestPassManager>();
            services.AddScoped<UserAnswerManager, UserAnswerManager>();
            services.AddScoped<UserTestResultManager, UserTestResultManager>();
        }
    }
}