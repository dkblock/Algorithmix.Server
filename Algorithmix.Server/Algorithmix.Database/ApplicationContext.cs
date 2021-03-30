using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Algorithmix.Entities;

namespace Algorithmix.Database
{
    public class ApplicationContext : IdentityDbContext<ApplicationUserEntity>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<ApplicationUserEntity> ApplicationUsers { get; set; }
        public DbSet<AlgorithmEntity> Algorithms { get; set; }
        public DbSet<AlgorithmTimeComplexityEntity> AlgorithmTimeComplexities { get; set; }
        public DbSet<GroupEntity> Groups { get; set; }
        public DbSet<TestEntity> Tests { get; set; }
        public DbSet<TestAnswerEntity> TestAnswers { get; set; }
        public DbSet<TestQuestionEntity> TestQuestions { get; set; }
    }
}
