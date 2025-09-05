using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Entities;
using Shared.Infrastructure.Interfaces;

namespace Shared.Infrastructure.Repositories;

public class GenericRepository<TContext, TEntity> : IGenericRepository<TEntity>
    where TEntity : BaseEntity where TContext : DbContext
{
    private readonly TContext _dbContext;

    public GenericRepository(TContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>()
            .FindAsync(id, cancellationToken);
    }

    public async Task<TEntity?> GetByFilterAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>()
            .FirstOrDefaultAsync(predicate, cancellationToken: cancellationToken);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>().ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Set<TEntity>().AsNoTracking();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.CountAsync(cancellationToken);
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
    }

    public void Update(TEntity entity)
    {
        _dbContext.Set<TEntity>().Update(entity);
    }

    public void Delete(TEntity entity)
    {
        _dbContext.Set<TEntity>().Remove(entity);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}