using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Persistence.EntityConfigurations;

internal class VacancyConfiguration : IEntityTypeConfiguration<Vacancy>
{
    public void Configure(EntityTypeBuilder<Vacancy> builder)
    {
        builder.HasOne(e => e.Leader)
            .WithMany()
            .HasForeignKey(e => e.LeaderId);

		builder.HasMany(e => e.Keywords)
			.WithMany();
    }
}
