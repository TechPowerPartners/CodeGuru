﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Api.Persistence;

public class ApplicationDbContext : DbContext
{
	protected readonly IConfiguration _configuration;

	public ApplicationDbContext(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public DbSet<User> Users { get; set; }
	public DbSet<Interview> Interviews { get; set; }
	public DbSet<Vacancy> Vacancies { get; set; }
	public DbSet<VacancyKeyword> VacancyKeywords { get; set; }
	public DbSet<Candidate> Candidates { get; set; }
    public DbSet<Test> Tests { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<QuestionFiles> QuestionFiles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseNpgsql(_configuration.GetConnectionString("ConnectionDb"));
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}
}
