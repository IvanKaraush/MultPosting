using Microsoft.EntityFrameworkCore;
using Shared.Domain.Enums;

namespace MultiPosting.Infrastructure;

public class MultiPostingDbContext : DbContext
{
    public MultiPostingDbContext(DbContextOptions<MultiPostingDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<SocialMedia>();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MultiPostingDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}