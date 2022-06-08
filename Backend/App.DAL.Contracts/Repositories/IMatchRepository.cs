using AppDalDto = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.DAL.Contracts.Repositories;

public interface IMatchRepository : IEntityRepository<AppDalDto.Match>, 
    IMatchRepositoryCustom<AppDalDto.Match>
{
    
}

public interface IMatchRepositoryCustom<TEntity>
{
    
}