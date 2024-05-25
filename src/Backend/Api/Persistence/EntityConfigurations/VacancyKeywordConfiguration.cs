using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Persistence.EntityConfigurations;

internal class VacancyKeywordConfiguration : IEntityTypeConfiguration<VacancyKeyword>
{
    public void Configure(EntityTypeBuilder<VacancyKeyword> builder)
    {
        builder.HasKey(e => e.Value);
    }
}
