using System.Security.Claims;
using Base.Contracts.DAL;
using Base.Contracts.Domain;

namespace Base.Contracts.BLL.Services;

public interface IBaseEntityService<TBllEntity, TDalEntity> : IBaseEntityService<TBllEntity, TDalEntity, Guid> 
    where TBllEntity : class, IDomainEntityId
    where TDalEntity : class, IDomainEntityId
{
    
}

public interface IBaseEntityService<TBllEntity, TDalEntity, TKey> : IEntityRepository<TBllEntity, TKey>, IBaseService
    where TBllEntity : class, IDomainEntityId<TKey>
    where TDalEntity : class, IDomainEntityId<TKey>
    where TKey : IEquatable<TKey>
{
    
}