using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultPosting.Domain.Entities;

namespace MultiPosting.Infrastructure.Configurations;

public class UserResourceConfiguration : IEntityTypeConfiguration<UserResource>
{
    public void Configure(EntityTypeBuilder<UserResource> builder)
    {
        builder.ToTable(nameof(UserResource));

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedNever();

        builder.Property(c => c.SocialMedia).HasColumnType("social_media");
    }
}