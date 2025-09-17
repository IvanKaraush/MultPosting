using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultPosting.Domain.Entities;

namespace MultiPosting.Infrastructure.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable(nameof(Project));

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedNever();

        builder.HasMany(p => p.UserResources)
            .WithOne()
            .HasForeignKey(w => w.ProjectId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}