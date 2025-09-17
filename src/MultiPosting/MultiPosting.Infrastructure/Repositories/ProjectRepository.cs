using Microsoft.EntityFrameworkCore;
using MultiPosting.Infrastructure.Interfaces;
using MultPosting.Domain.Entities;
using Shared.Infrastructure.Repositories;

namespace MultiPosting.Infrastructure.Repositories;

public class ProjectRepository : GenericRepository<MultiPostingDbContext, Project>, IProjectRepository
{
    private readonly MultiPostingDbContext _context;
    public ProjectRepository(MultiPostingDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Project?> GetFullById(Guid id)
    {
        return await _context.Set<Project>().Include(c => c.UserResources).FirstOrDefaultAsync(c => c.Id == id);
    }
}