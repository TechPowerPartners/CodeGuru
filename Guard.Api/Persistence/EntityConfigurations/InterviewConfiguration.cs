using Guard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Guard.Api.Persistence.EntityConfigurations;

internal class InterviewConfiguration : IEntityTypeConfiguration<Interview>
{
    public void Configure(EntityTypeBuilder<Interview> builder)
    {
        builder.HasOne(e => e.Interviewee)
            .WithMany()
            .HasForeignKey(e => e.IntervieweeId);

        builder.ComplexProperty(e => e.Date);
        builder.ComplexProperty(e => e.Role);
    }
}
