using Base.Contracts.Domain;

namespace Base.Contracts.DAL;

public interface IEntityRepository<TEntity> : IEntityRepository<TEntity, Guid> 
    where TEntity : class, IDomainEntityId
{
    
}

public interface IEntityRepository<TEntity, TKey> 
    where TEntity : class, IDomainEntityId<TKey> 
    where TKey : IEquatable<TKey>
{
    TEntity Add(TEntity entity);
    
    TEntity Update(TEntity entity);

    // async

    Task<TEntity> RemoveAsync(TKey id);

    Task<TEntity?> FirstOrDefaultAsync(TKey id, TKey? userId = default!, bool noTracking = true);

    Task<IEnumerable<TEntity>> GetAllAsync(TKey? userId = default ,bool noTracking = true);

    Task<bool> ExistsAsync(TKey id);

    
}