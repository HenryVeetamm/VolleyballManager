using Base.Contracts.DAL;
using Base.Contracts.DAL.Mappers;
using Base.Contracts.Domain;
using Microsoft.EntityFrameworkCore;

namespace Base.DAL.EF;




public class BaseEntityRepository<TDalEntity, TDomainEntity, TDbContext> 
    : BaseEntityRepository<TDalEntity, TDomainEntity, Guid, TDbContext>
    where TDalEntity : class, IDomainEntityId<Guid>
    where TDomainEntity : class, IDomainEntityId<Guid>
    where TDbContext : DbContext

{
    public BaseEntityRepository(TDbContext dbContext, IBaseMapper<TDalEntity, TDomainEntity> mapper) : base(dbContext, mapper)
    {
    }
}

public class BaseEntityRepository<TDalEntity, TDomainEntity, TKey, TDbContext> : 
    IEntityRepository<TDalEntity, TKey>
    where TDalEntity : class, IDomainEntityId<TKey>
    where TDomainEntity : class, IDomainEntityId<TKey>
    where TKey : IEquatable<TKey>
    where TDbContext : DbContext
{
    protected readonly TDbContext RepoDbContext;
    protected readonly DbSet<TDomainEntity> RepoDbSet;
    protected readonly IBaseMapper<TDalEntity, TDomainEntity> Mapper;
 

    public BaseEntityRepository(TDbContext dbContext, IBaseMapper<TDalEntity, TDomainEntity> mapper)
    {
        RepoDbContext = dbContext;
        RepoDbSet = dbContext.Set<TDomainEntity>();
        Mapper = mapper;
    }
    
    public virtual TDalEntity Add(TDalEntity entity)
    {
        return Mapper.Map(RepoDbSet.Add(Mapper.Map(entity)!).Entity)!;
    }

    public virtual TDalEntity Update(TDalEntity entity)
    {
        var domainEntity = Mapper.Map(entity);
        var updatedDomainEntity = RepoDbSet.Update(domainEntity!).Entity;
        var dalEntity = Mapper.Map(updatedDomainEntity);
        /*return Mapper.Map(RepoDbSet.Update(Mapper.Map(entity)!).Entity!)!;*/
        return dalEntity!;
    }

    public virtual async Task<TDalEntity> RemoveAsync(TKey id)
    {
        var entity = await FirstOrDefaultAsync(id);

        if (entity == null)
        {
            throw new NullReferenceException($"Entity {typeof(TDomainEntity).Name} with id ${id} was not found");
        }

        return Mapper.Map(RepoDbSet.Remove(Mapper.Map(entity)!).Entity)!;;
    }

    public virtual async Task<TDalEntity?> FirstOrDefaultAsync(TKey id, TKey? userId = default, bool noTracking = true)
    {
        var query = CreateQuery(userId, noTracking);
        
        return Mapper.Map(await query.FirstOrDefaultAsync(a => a.Id.Equals(id)));
    }

    public virtual async Task<IEnumerable<TDalEntity>> GetAllAsync(TKey? userId = default ,bool noTracking = true)
    {
        var query = CreateQuery(userId, noTracking);
        
        return await query.Select(domainEntity => Mapper.Map(domainEntity)!).ToListAsync();
    }

    public virtual async Task<bool> ExistsAsync(TKey id)
    {
        return await RepoDbSet.AnyAsync(a => a.Id.Equals(id));
    }

    public virtual IQueryable<TDomainEntity> CreateQuery(TKey? userId = default, bool noTracking = true)
    {
        var query = RepoDbSet.AsQueryable();

       
        if (userId != null && !userId.Equals(default) &&
            typeof(IDomainAppUserId<TKey>).IsAssignableFrom(typeof(TDomainEntity)))
        {
            query = query.Where(e => ((IDomainAppUserId<TKey>)e).AppUserId.Equals(userId));
        }

        if (noTracking)
        {
            query = query.AsNoTracking();
        }

        return query;
    }
}