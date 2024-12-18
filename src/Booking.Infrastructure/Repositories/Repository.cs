using Booking.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Booking.Infrastructure.Repositories
{
    internal class Repository<TEntity, TEntityId> : IRepository<TEntity, TEntityId> where TEntity : Entity<TEntityId>
        where TEntityId : class, IEntityId<TEntityId>
    {
        private readonly ApplicationDbContext _dbContext;

        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
        }

        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var entityAdded = await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);

            return entityAdded.Entity;
        }

        public async Task<TEntity?> GetByIdAsync(TEntityId id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
        }
    }
}
