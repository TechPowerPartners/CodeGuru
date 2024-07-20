using Microsoft.EntityFrameworkCore;
using TestingPlatform.Domain.Entities;

namespace TestingPlatform.Api.Persistence;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options
    ) : DbContext(options)
{
    public DbSet<Test> Tests { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<QuestionFile> QuestionFiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("testingPlatform");
    }
}
