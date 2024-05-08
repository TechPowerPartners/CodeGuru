using Guard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Guard.Api.Persistence;

public class ApplicationDbContext : DbContext
{
	protected readonly IConfiguration _configuration;

	public ApplicationDbContext(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public DbSet<User> Users { get; set; }
	public DbSet<Interview> Interviews { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseNpgsql(_configuration.GetConnectionString("ConnectionDb"));
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}
}
