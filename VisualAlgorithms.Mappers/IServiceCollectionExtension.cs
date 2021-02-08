using Microsoft.Extensions.DependencyInjection;

namespace VisualAlgorithms.Mappers
{
    public static class IServiceCollectionExtension
    {
        public static void AddMappers(this IServiceCollection services)
        {
            services.AddSingleton<TestsMapper, TestsMapper>();
            services.AddSingleton<TestAnswersMapper, TestAnswersMapper>();
            services.AddSingleton<TestQuestionsMapper, TestQuestionsMapper>();
        }
    }
}
