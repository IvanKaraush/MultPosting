using IdentityServer.Domain.Entities;
using Shared.Infrastructure.Interfaces;

namespace IdentityServer.Infrastructure.Interfaces;

public interface IAccessTokenRepository : IGenericRepository<AccessToken>
{
    
}