using MultPosting.Domain.Entities;
using Shared.Infrastructure.Interfaces;

namespace MultiPosting.Infrastructure.Interfaces;

public interface IProjectRepository : IGenericRepository<Project>
{
    Task<Project?> GetFullById(Guid id);
}