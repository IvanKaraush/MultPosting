using Microsoft.EntityFrameworkCore;

namespace MultiPosting.Infrastructure;

public class MultiPostingDbContext : DbContext
{
    public MultiPostingDbContext(DbContextOptions<MultiPostingDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MultiPostingDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}