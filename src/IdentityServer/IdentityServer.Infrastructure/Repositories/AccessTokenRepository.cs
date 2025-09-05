using IdentityServer.Domain.Entities;
using IdentityServer.Infrastructure.Interfaces;
using Shared.Infrastructure.Repositories;

namespace IdentityServer.Infrastructure.Repositories;

public class AccessTokenRepository : GenericRepository<ApplicationIdentityDbContext, AccessToken>,
    IAccessTokenRepository
{
    public AccessTokenRepository(ApplicationIdentityDbContext context) : base(context)
    {
    }
}