using Base.Contracts.BLL.Mappers;
using Base.Contracts.BLL.Services;
using Base.Contracts.DAL;

using Base.Contracts.Domain;

namespace Base.BLL.Services;

public class BaseEntityService<TUnitOfWork, TRepository, TBllEntity, TDalEntity> : 
    BaseEntityService<TUnitOfWork, TRepository, TBllEntity, TDalEntity, Guid>
    , IBaseEntityService<TBllEntity, TDalEntity>
    where TDalEntity : class, IDomainEntityId
    where TBllEntity : class, IDomainEntityId
    where TUnitOfWork : IUnitOfWork
    where TRepository : IEntityRepository<TDalEntity>
{
    public BaseEntityService(TUnitOfWork serviceUow, TRepository serviceRepository,
        IBaseMapper<TBllEntity, TDalEntity> mapper) 
        : base(serviceUow, serviceRepository, mapper)
    {
      
    }
}


public class BaseEntityService<TUnitOfWork, TRepository, TBllEntity, TDalEntity, TKey> :
    IBaseEntityService<TBllEntity, TDalEntity, TKey>
    where TBllEntity : class, IDomainEntityId<TKey>
    where TDalEntity : class, IDomainEntityId<TKey>
    where TKey : IEquatable<TKey>
    where TUnitOfWork : IUnitOfWork
    where TRepository : IEntityRepository<TDalEntity, TKey>
{

    protected TUnitOfWork ServiceUow;
    protected TRepository ServiceRepository;
    protected IBaseMapper<TBllEntity, TDalEntity> Mapper;

    public BaseEntityService(TUnitOfWork serviceUow, TRepository serviceRepository, IBaseMapper<TBllEntity, TDalEntity> mapper)
    {
        ServiceUow = serviceUow;
        ServiceRepository = serviceRepository;
        Mapper = mapper;
    }
    
    public TBllEntity Add(TBllEntity entity)
    {
        return Mapper.Map(ServiceRepository.Add(Mapper.Map(entity)!))!;
    }

    public TBllEntity Update(TBllEntity entity)
    {
        return Mapper.Map(ServiceRepository.Update(Mapper.Map(entity)!))!;
    }

    public async Task<TBllEntity> RemoveAsync(TKey id)
    {
        return Mapper.Map(await ServiceRepository.RemoveAsync(id))!;
    }

    public async Task<TBllEntity?> FirstOrDefaultAsync(TKey id, TKey? userId = default, bool noTracking = true)
    {
        return Mapper.Map(await ServiceRepository.FirstOrDefaultAsync(id, userId, noTracking));
    }

    public async Task<IEnumerable<TBllEntity>> GetAllAsync(TKey? userId = default, bool noTracking = true)
    {
        return (await ServiceRepository.GetAllAsync(userId, noTracking)).Select(x => Mapper.Map(x)!);
    }

    public async Task<bool> ExistsAsync(TKey id)
    {
        return await ServiceRepository.ExistsAsync(id);
    }
}