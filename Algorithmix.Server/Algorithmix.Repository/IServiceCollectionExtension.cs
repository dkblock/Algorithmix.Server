﻿using Microsoft.Extensions.DependencyInjection;

namespace Algorithmix.Repository
{
    public static class IServiceCollectionExtension
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<AlgorithmRepository, AlgorithmRepository>();
            services.AddScoped<AlgorithmTimeComplexityRepository, AlgorithmTimeComplexityRepository>();
            services.AddScoped<GroupRepository, GroupRepository>();
            services.AddScoped<TestRepository, TestRepository>();
            services.AddScoped<TestAnswerRepository, TestAnswerRepository>();
            services.AddScoped<TestQuestionRepository, TestQuestionRepository>();
            services.AddScoped<UserAnswerRepository, UserAnswerRepository>();
            services.AddScoped<UserTestResultRepository, UserTestResultRepository>();
        }
    }
}
