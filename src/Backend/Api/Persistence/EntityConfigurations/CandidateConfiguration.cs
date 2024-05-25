using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Persistence.EntityConfigurations;

internal class CandidateConfiguration : IEntityTypeConfiguration<Candidate>
{
    public void Configure(EntityTypeBuilder<Candidate> builder)
    {
        builder.HasKey(e => new { e.VacancyId, e.UserId });

        builder.HasOne(e => e.User)
			.WithMany()
			.HasForeignKey(e => e.UserId);

        builder.HasOne(e => e.Vacancy)
            .WithMany()
            .HasForeignKey(e => e.VacancyId);
    }
}