using IdentityServer.Domain.Entities;
using IdentityServer.Infrastructure.Context;
using IdentityServer.Infrastructure.Interfaces;
using Shared.Infrastructure.Repositories;

namespace IdentityServer.Infrastructure.Repositories;

public class AccessTokenRepository : GenericRepository<ApplicationDbContext, AccessToken>,
    IAccessTokenRepository
{
    public AccessTokenRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}