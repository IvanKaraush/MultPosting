using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Infrastructure;

public class ApplicationIdentityDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options) : base(options)
    {
    }
}