namespace Booking.Domain.Abstractions
{
    public interface IRepository<TEntity, TEntityId> where TEntity : Entity<TEntityId>
    {
        Task<TEntity?> GetByIdAsync(TEntityId id, CancellationToken cancellationToken = default);
        void Add(TEntity entity);
        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    }
}
