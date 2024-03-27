using botovskixAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace botovskixAPI.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        protected readonly IConfiguration _configuration;

        public ApplicationDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("ConnectionDb"));
        }

        public DbSet<Users> Users { get; set; }

        public DbSet<Posts> Posts { get; set; }

    }
}
